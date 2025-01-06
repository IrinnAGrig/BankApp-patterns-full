using Backend_BankingApp.Model;
using Backend_BankingApp.Utilities.Flyweight;
using MediatR;

namespace Backend_BankingApp.Requests.TransactionsRequest
{
    public class GetServicesRequest : IRequest<(bool status, List<Service> list)>
    {
          internal class GetHandler : IRequestHandler<GetServicesRequest, (bool status, List<Service>)>
          {
               private readonly ServiceFlyweightFactory _serviceFactory;
               private readonly ILogger<GetHandler> _logger;

               public GetHandler(ServiceFlyweightFactory serviceFactory, ILogger<GetHandler> logger)
               {
                    _serviceFactory = serviceFactory;
                    _logger = logger;
               }

               public async Task<(bool status, List<Service>)> Handle(GetServicesRequest request, CancellationToken cancellationToken)
               {
                    try
                    {
                         var flyweightServices = await _serviceFactory.GetAllServicesAsync();

                         var services = flyweightServices
                             .Where(flyweight => flyweight.IsValid())
                             .Select(flyweight => new Service
                             {
                                  IdService = flyweight.IdService,
                                  Title = flyweight.Title,
                                  Subtitle = flyweight.Subtitle,  
                                  Emblem = flyweight.Emblem
                             })
                             .ToList();

                         return (true, services);
                    }
                    catch (Exception ex)
                    {
                         _logger.LogError("Error retrieving services: {ErrorMessage}", ex.Message);
                         return (false, null);
                    }
               }
          }
     }
}
