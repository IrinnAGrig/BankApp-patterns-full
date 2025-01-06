using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.CardRequest
{
    public class FindCardByNumberRequest : IRequest<(bool status, Card card)>
    {
        public string CardNumber { get; set; }
        public FindCardByNumberRequest(string cardNumber)
        {
            CardNumber = cardNumber;
        }

        internal class GetHandler : IRequestHandler<FindCardByNumberRequest, (bool status, Card card)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, Card card)> Handle(FindCardByNumberRequest request, CancellationToken cancellationToken)
            {
                try
                {
                    Card card = await _context.Cards
                        .FirstOrDefaultAsync(c => c.CardNumber == request.CardNumber, cancellationToken);

                    if (card == null)
                    {
                        _logger.LogWarning("Card with number {CardNumber} not found.", request.CardNumber);
                        return (false, null); 
                    }

                    return (true, card); 
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while retrieving card with number {CardNumber}: {ErrorMessage}", request.CardNumber, ex.Message);
                    return (false, null); 
                }
            }
        }
    }

}
