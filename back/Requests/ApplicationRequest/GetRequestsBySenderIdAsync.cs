using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.ApplicationRequest
{
    public class GetRequestsBySenderIdAsync : IRequest<(bool status, List<RequestModel> list)>
    {
        public string Id { get; set; }
        public GetRequestsBySenderIdAsync(string id)
        {
            Id = id;
        }
        internal class GetHandler : IRequestHandler<GetRequestsBySenderIdAsync, (bool status, List<RequestModel>)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, List<RequestModel>)> Handle(GetRequestsBySenderIdAsync request, CancellationToken cancellationToken)
            {
                try
                {
                    List<RequestModel> requests = new List<RequestModel>();

                    requests = await _context.RequestsMoney
                            .Where(g => EF.Functions.Like(g.SenderId, request.Id))
                            .ToListAsync();


                    return (true, requests);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Unable to create category: {title}. Error: {message}", ex.Message);
                    return (false, null);
                }
            }
        }
    }
}
