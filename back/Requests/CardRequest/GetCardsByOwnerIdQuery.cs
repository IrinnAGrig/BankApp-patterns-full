using Azure.Core;
using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.CardRequest
{
    public class GetCardsByOwnerIdQuery : IRequest<(bool status, List<Card> list)>
    {
        public string Id { get; set; }
        public GetCardsByOwnerIdQuery(string id)
        {
            Id = id;
        }
        internal class GetHandler : IRequestHandler<GetCardsByOwnerIdQuery, (bool status, List<Card>)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, List<Card>)> Handle(GetCardsByOwnerIdQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    List<Card> cards = new List<Card>();

                    cards = await _context.Cards
                            .Where(g => EF.Functions.Like(g.OwnerId, request.Id))
                            .ToListAsync();
                    

                    return (true, cards);
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
