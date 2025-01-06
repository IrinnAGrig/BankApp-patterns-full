using Backend_BankingApp.Utilities.Adapter;
using Backend_BankingApp.Utilities.ChainOfResponsability;
using Backend_BankingApp.Utilities.Observer;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using MediatR;

namespace BankingAppBackend.Requests.TransactionsRequest
{
    public class AddTransactionS : IRequest<bool>
    {
        public TransactionSupplyDto Data { get; set; }

        public AddTransactionS(TransactionSupplyDto data)
        {
            Data = data ?? throw new ArgumentNullException(nameof(data));
        }

        internal class Handler : IRequestHandler<AddTransactionS, bool>
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

               public async Task<bool> Handle(AddTransactionS request, CancellationToken cancellationToken)
               {
                    try
                    {
                         var cardValidationHandler = new CardValidationHandler(_context);
                         var transactionSaveHandler = new TransactionSaveHandler(_context);
                         var notificationHandler = new NotificationHandler(_webSocketService, new TransactionAdapter(_context));

                         cardValidationHandler.SetNext(transactionSaveHandler);
                         transactionSaveHandler.SetNext(notificationHandler);

                         var result = await cardValidationHandler.HandleAsync(request.Data, cancellationToken);

                         return result;
                    }
                    catch (Exception ex)
                    {
                         _logger.LogError("Error processing transaction: {ErrorMessage}", ex.Message);
                         return false;
                    }
               }

          }
     }
   

}
