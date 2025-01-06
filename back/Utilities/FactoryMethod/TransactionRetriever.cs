using Backend_BankingApp.Utilities.Adapter;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using Microsoft.EntityFrameworkCore;

namespace Backend_BankingApp.Utilities.FactoryMethod
{
     public abstract class TransactionRetrieverFactoryBase
     {
          public abstract TransactionRetriever Create();
     }

     public class BuyTransactionRetrieverFactory : TransactionRetrieverFactoryBase
     {
          private readonly BankDbContext _context;
          public BuyTransactionRetrieverFactory(BankDbContext context) => _context = context;

          public override TransactionRetriever Create()
          {
               return new BuyTransactionRetriever(_context);
          }
     }

     public class SupplyTransactionRetrieverFactory : TransactionRetrieverFactoryBase
     {
          private readonly BankDbContext _context;
          public SupplyTransactionRetrieverFactory(BankDbContext context) => _context = context;

          public override TransactionRetriever Create()
          {
               return new SupplyTransactionRetriever(_context);
          }
     }

     public class TransferTransactionRetrieverFactory : TransactionRetrieverFactoryBase
     {
          private readonly BankDbContext _context;
          public TransferTransactionRetrieverFactory(BankDbContext context) => _context = context;

          public override TransactionRetriever Create()
          {
               return new TransferTransactionRetriever(_context);
          }
     }

     public class AllTransactionRetrieverFactory : TransactionRetrieverFactoryBase
     {
          private readonly BankDbContext _context;
          public AllTransactionRetrieverFactory(BankDbContext context) => _context = context;

          public override TransactionRetriever Create()
          {
               return new AllTransactionRetriever(_context);
          }
     }

    public abstract class TransactionRetriever
     {
          protected readonly BankDbContext _context;

          protected TransactionRetriever(BankDbContext context)
          {
               _context = context;
          }

          public abstract Task<List<TransactionShow>> GetTransactionsShow(string userId, CancellationToken cancellationToken);
          public abstract Task<List<Notification>> GetTransactionsNotif(string userId, CancellationToken cancellationToken);
     }

     public class BuyTransactionRetriever : TransactionRetriever
     {
          private readonly ITransactionAdapter _adapter;

          public BuyTransactionRetriever(BankDbContext context) : base(context)
          {
               _adapter = new TransactionAdapter(context);
          }

          public override async Task<List<TransactionShow>> GetTransactionsShow(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsBuy
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.Adapt(t)).ToList();
          }

          public override async Task<List<Notification>> GetTransactionsNotif(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsBuy
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.AdaptNotif(t)).ToList();
          }
     }

     public class SupplyTransactionRetriever : TransactionRetriever
     {
          private readonly ITransactionAdapter _adapter;

          public SupplyTransactionRetriever(BankDbContext context) : base(context)
          {
               _adapter = new TransactionAdapter(context);
          }

          public override async Task<List<TransactionShow>> GetTransactionsShow(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsSupply
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.Adapt(t)).ToList();
          }

          public override async Task<List<Notification>> GetTransactionsNotif(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsSupply
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.AdaptNotif(t)).ToList();
          }
     }

     public class TransferTransactionRetriever : TransactionRetriever
     {
          private readonly ITransactionAdapter _adapter;

          public TransferTransactionRetriever(BankDbContext context) : base(context)
          {
               _adapter = new TransactionAdapter(context);
          }

          public override async Task<List<TransactionShow>> GetTransactionsShow(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsTransfer
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.Adapt(t)).ToList();
          }

          public override async Task<List<Notification>> GetTransactionsNotif(string userId, CancellationToken cancellationToken)
          {
               var transactions = await _context.TransactionsTransfer
                   .Where(t => t.SenderId == userId)
                   .ToListAsync(cancellationToken);

               return transactions.Select(t => _adapter.AdaptNotif(t)).ToList();
          }
     }

     public class AllTransactionRetriever : TransactionRetriever
     {
          private readonly List<TransactionRetriever> _retrievers;

          public AllTransactionRetriever(BankDbContext context) : base(context)
          {
               _retrievers = new List<TransactionRetriever>
            {
                new BuyTransactionRetriever(context),
                new SupplyTransactionRetriever(context),
                new TransferTransactionRetriever(context)
            };
          }

          public override async Task<List<TransactionShow>> GetTransactionsShow(string userId, CancellationToken cancellationToken)
          {
               var allTransactions = new List<TransactionShow>();

               foreach (var retriever in _retrievers)
               {
                    var transactions = await retriever.GetTransactionsShow(userId, cancellationToken);
                    allTransactions.AddRange(transactions);
               }

               return allTransactions.OrderByDescending(t => t.Date).ToList();
          }

          public override async Task<List<Notification>> GetTransactionsNotif(string userId, CancellationToken cancellationToken)
          {
               var allNotifications = new List<Notification>();

               foreach (var retriever in _retrievers)
               {
                    var notifications = await retriever.GetTransactionsNotif(userId, cancellationToken);
                    allNotifications.AddRange(notifications);
               }

               return allNotifications.OrderByDescending(t => t.Timestamp).ToList();
          }
     }
}
