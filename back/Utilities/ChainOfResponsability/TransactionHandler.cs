using Backend_BankingApp.Model;
using Backend_BankingApp.Utilities.Adapter;
using Backend_BankingApp.Utilities.Observer;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Backend_BankingApp.Utilities.ChainOfResponsability
{
     public interface ITransactionHandler<T>
     {
          Task<bool> HandleAsync(T transaction, CancellationToken cancellationToken);
          void SetNext(ITransactionHandler<T> nextHandler);
     }

     public abstract class TransactionHandlerBase<T> : ITransactionHandler<T>
     {
          private ITransactionHandler<T> _nextHandler;

          public void SetNext(ITransactionHandler<T> nextHandler)
          {
               _nextHandler = nextHandler;
          }

          public virtual async Task<bool> HandleAsync(T transaction, CancellationToken cancellationToken)
          {
               if (_nextHandler != null)
               {
                    return await _nextHandler.HandleAsync(transaction, cancellationToken);
               }

               return true; 
          }
     }

     public class CardValidationHandler : TransactionHandlerBase<TransactionDto>
     {
          private readonly BankDbContext _context;

          public CardValidationHandler(BankDbContext context)
          {
               _context = context;
          }

          public override async Task<bool> HandleAsync(TransactionDto transaction, CancellationToken cancellationToken)
          {
               var selectedCard = await _context.Cards
                   .FirstOrDefaultAsync(c => c.Id == transaction.CardId && c.OwnerId == transaction.SenderId, cancellationToken);

               if (selectedCard == null)
               {
                    return false;
               }

               if (selectedCard.Balance < transaction.Sum)
               {
                    return false;
               }

               selectedCard.Balance -= transaction.Sum;
               _context.Cards.Update(selectedCard);
               await _context.SaveChangesAsync(cancellationToken);

               return await base.HandleAsync(transaction, cancellationToken); 
          }
     }

     public class TransactionSaveHandler : TransactionHandlerBase<TransactionDto>
     {
          private readonly BankDbContext _context;

          public TransactionSaveHandler(BankDbContext context)
          {
               _context = context;
          }

          public override async Task<bool> HandleAsync(TransactionDto transaction, CancellationToken cancellationToken)
          {
               if (transaction is TransactionSupplyDto supplyRequest)
               {
                    var newTransaction = new TransactionSupply
                    {
                         SenderId = supplyRequest.SenderId,
                         CardId = supplyRequest.CardId,
                         Sum = supplyRequest.Sum,
                         Valute = supplyRequest.Valute
                    };
                    await _context.TransactionsSupply.AddAsync(newTransaction, cancellationToken);
               }
               else if (transaction is TransactionTransferDto transferRequest)
               {
                    var newTransaction = new TransactionTransfer
                    {
                         SenderId = transferRequest.SenderId,
                         CardId = transferRequest.CardId,
                         ReceiverCardId = transferRequest.ReceiverCardId,
                         ReceiverId = transferRequest.ReceiverId,
                         Sum = transferRequest.Sum,
                         Valute = transferRequest.Valute
                    };
                    await _context.TransactionsTransfer.AddAsync(newTransaction, cancellationToken);
               }

               await _context.SaveChangesAsync(cancellationToken);

               return await base.HandleAsync(transaction, cancellationToken); 
          }
     }

     public class NotificationHandler : TransactionHandlerBase<TransactionDto>
     {
          private readonly WebSocketService _webSocketService;
          private readonly TransactionAdapter _transactionAdapter;

          public NotificationHandler(WebSocketService webSocketService, TransactionAdapter transactionAdapter)
          {
               _webSocketService = webSocketService;
               _transactionAdapter = transactionAdapter;
          }

          public override async Task<bool> HandleAsync(TransactionDto transaction, CancellationToken cancellationToken)
          {
               if (transaction == null)
               {
                    throw new ArgumentNullException(nameof(transaction), "Transaction cannot be null.");
               }

               var notification = _transactionAdapter.AdaptNotif(transaction);

               if (notification == null)
               {
                    throw new InvalidOperationException("Notification adaptation failed.");
               }

               var messageJson = JsonSerializer.Serialize(notification);
               await _webSocketService.SendMessageToClient(transaction.SenderId, messageJson);

               return await base.HandleAsync(transaction, cancellationToken);
          }
     }
}
