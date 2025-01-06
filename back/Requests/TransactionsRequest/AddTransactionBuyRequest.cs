using Backend_BankingApp.Model;
using Backend_BankingApp.Utilities.Adapter;
using Backend_BankingApp.Utilities.ChainOfResponsability;
using Backend_BankingApp.Utilities.Observer;
using Backend_BankingApp.Utilities.State;
using Backend_BankingApp.Utilities.Strategy;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.TransactionsRequest
{
    public class AddTransactionB : IRequest<bool>
    {
        public TransactionBuyDto Data { get; set; }

        public AddTransactionB(TransactionBuyDto data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        internal class Handler : IRequestHandler<AddTransactionB, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;
            private readonly WebSocketService _webSocketService;

            public Handler(BankDbContext context, ILogger<Handler> logger, WebSocketService webSocketService)
            {
                _context = context;
                _logger = logger;
                _webSocketService = webSocketService;
            }

            public async Task<bool> Handle(AddTransactionB request, CancellationToken cancellationToken)
            {
                try
                {
                    var selectedCard = await _context.Cards
                        .FirstOrDefaultAsync(c => c.Id == request.Data.CardId && c.OwnerId == request.Data.SenderId, cancellationToken);

                    if (selectedCard == null)
                    {
                        _logger.LogWarning("The specified card was not found or does not belong to the user.");
                        return false;
                    }
                    IPaymentStrategy paymentStrategy;

                    if (selectedCard.TypeCard == "Debit")
                    {
                        paymentStrategy = new DebitCardPaymentStrategy();
                    }
                    else if (selectedCard.TypeCard == "Credit")
                    {
                                ICreditCardState cardState = StateFactory.GetState(selectedCard.Type); 
                                paymentStrategy = new CreditCardPaymentStrategy(cardState);
                         }
                    else
                    {
                        _logger.LogWarning("Card type is invalid.");
                        return false;
                    }

                    var finalAmount = paymentStrategy.ApplyPayment(selectedCard, request.Data.Sum, request.Data.ServiceId);

                    selectedCard.Balance -= finalAmount;

                    _context.Cards.Update(selectedCard);

                    var transaction = new TransactionBuy(
                         senderId: request.Data.SenderId,
                         sum: finalAmount,
                         valute: request.Data.Valute,
                         serviceId: request.Data.ServiceId,
                         cardId: request.Data.CardId
                     );

                         transaction.RemainAmount = selectedCard.Balance;

                    await _context.TransactionsBuy.AddAsync(transaction, cancellationToken);

                    var result = await _context.SaveChangesAsync(cancellationToken);
                    if (result > 0)
                    {
                         var notificationHandler = new NotificationHandler(_webSocketService, new TransactionAdapter(_context));
                         var done = await notificationHandler.HandleAsync(request.Data, cancellationToken);

                         return done;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Eroare la procesarea tranzacției de tip 'TransactionBuy': {ErrorMessage}", ex.Message);
                    return false;
                }
            }

          }
     }

}
