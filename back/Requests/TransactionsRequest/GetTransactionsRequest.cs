using Backend_BankingApp.Utilities.FactoryMethod;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using MediatR;

namespace BankingAppBackend.Requests.TransactionsRequest
{
    public class GetTransactionsRequest : IRequest<(bool status, List<TransactionShow> list)>
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public GetTransactionsRequest(string id, string type)
        {
            Id = id;    
            Type = type;
        }
        internal class GetHandler : IRequestHandler<GetTransactionsRequest, (bool status, List<TransactionShow>)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

               public async Task<(bool status, List<TransactionShow>)> Handle(GetTransactionsRequest request, CancellationToken cancellationToken)
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
                         var transactions = await retriever.GetTransactionsShow(request.Id, cancellationToken);


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
