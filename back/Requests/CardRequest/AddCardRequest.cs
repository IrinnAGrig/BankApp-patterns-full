using Backend_BankingApp.Utilities.Builder;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;
using MediatR;

namespace BankingAppBackend.Requests.CardRequest
{
    public class AddCardRequest : IRequest<bool>
    {
        public CardDTO Card { get; set; }

        public AddCardRequest(CardDTO card)
        {
            Card = card;
        }

        internal class AddCardHandler : IRequestHandler<AddCardRequest, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<AddCardHandler> _logger;

            public AddCardHandler(BankDbContext context, ILogger<AddCardHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(AddCardRequest request, CancellationToken cancellationToken)
            {
                try
                {
                         CardBuildDirector director;
                         Card card;

                         if (request.Card.TypeCard == "Debit")
                         {
                              director = new CardBuildDirector(new DebitCardBuilder());
                         }
                         else if (request.Card.TypeCard == "Credit")
                         {
                              director = new CardBuildDirector(new CreditCardBuilder());
                         }
                         else
                         {
                              throw new ArgumentException("Card type is invalid");
                         }

                         director.Construct(request.Card);
                         card = director.GetResult();

                    if (card.Network == null) card.Network = "none";
                    
                    await _context.Cards.AddAsync(card, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken);

                    _logger.LogInformation("Card number {CardNumber} was added successfully", card.CardNumber);

                    return result > 0;
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while adding the card: {ErrorMessage}", ex.Message);
                    return false;
                }

            }
        }
    }

}
