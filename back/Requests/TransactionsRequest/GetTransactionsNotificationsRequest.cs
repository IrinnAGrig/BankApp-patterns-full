using Backend_BankingApp.Utilities.FactoryMethod;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using MediatR;

namespace Backend_BankingApp.Requests.TransactionsRequest
{
    public class GetTransactionsNotificationsRequest : IRequest<(bool status, List<Notification> list)>
     {
          public string Id { get; set; }
          public string Type { get; set; }
          public GetTransactionsNotificationsRequest(string id, string type)
          {
               Id = id;
               Type = type;
          }
          internal class GetHandler : IRequestHandler<GetTransactionsNotificationsRequest, (bool status, List<Notification>)>
          {
               private readonly BankDbContext _context;
               private readonly ILogger<GetHandler> _logger;

               public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
               {
                    _context = context;
                    _logger = logger;
               }

               public async Task<(bool status, List<Notification>)> Handle(GetTransactionsNotificationsRequest request, CancellationToken cancellationToken)
               {
                    try
                    {
                         TransactionRetrieverFactoryBase factory;

                         switch (request.Type.ToLower())
                         {
                              case "buy":
                                   factory = new BuyTransactionRetrieverFactory(_context);
                                   break;
                              case "supply":
                                   factory = new SupplyTransactionRetrieverFactory(_context);
                                   break;
                              case "transfer":
                                   factory = new TransferTransactionRetrieverFactory(_context);
                                   break;
                              case "all":
                                   factory = new AllTransactionRetrieverFactory(_context);
                                   break;
                              default:
                                   throw new ArgumentException("Invalid transaction type.");
                         }

                         var retriever = factory.Create();

                         var transactions = await retriever.GetTransactionsNotif(request.Id, cancellationToken);

                         return (true, transactions);
                    }
                    catch (Exception ex)
                    {
                         _logger.LogError("Unable to fetch transactions for user {Id}. Error: {Message}", request.Id, ex.Message);
                         return (false, null);
                    }
               }

          }
     }
}
