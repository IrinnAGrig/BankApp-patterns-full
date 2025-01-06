using Azure.Core;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.CardRequest
{
    public class UpdateCardCommand : IRequest<bool>
    {
          public CardUpdateDto Card { get; set; }
        public string Id { get; set; }

        public UpdateCardCommand(string id, CardUpdateDto card)
        {
            Card = card;
            Id = id;
        }

        internal class UpdateCardHandler : IRequestHandler<UpdateCardCommand, bool>
        {
            private readonly BankDbContext _context;

            public UpdateCardHandler(BankDbContext context)
            {
                _context = context;
            }

            public async Task<bool> Handle(UpdateCardCommand request, CancellationToken cancellationToken)
            {
                try
                {

                         var course = await _context.Cards.FindAsync(request.Id);

                         if (course != null)
                         {
                              var errors = ValidateCardData(request.Card);
                              if (errors.Any())
                              {
                                   throw new ArgumentException($"Invalid data: {string.Join(", ", errors)}");
                              }

                              var updatedCard = course.Clone();

                              updatedCard = MapToCard((Card)updatedCard, request.Card);

                              _context.Entry(course).CurrentValues.SetValues(updatedCard);
                         }
                         else
                         {
                              return false;
                         }

                         await _context.SaveChangesAsync();

                         return true;
                    }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
          private static List<string> ValidateCardData(CardUpdateDto dto)
          {
               var errors = new List<string>();

               if (dto.Balance < 0)
               {
                    errors.Add("Balance cannot be negative.");
               }

               DateTime expiryDate;
               if (!DateTime.TryParse(dto.ExpiryDate, out expiryDate))
               {
                    errors.Add("ExpiryDate is not a valid date.");
               }
               else if (expiryDate < DateTime.UtcNow)
               {
                    errors.Add("ExpiryDate must be in the future.");
               }

               return errors;
          }

          public static Card MapToCard(Card existingCard, CardUpdateDto dto)
          {
               existingCard.TypeCard = dto.TypeCard ?? existingCard.TypeCard;
               existingCard.Type = dto.Type ?? existingCard.Type;
               existingCard.Valute = dto.Valute ?? existingCard.Valute;
               existingCard.Balance = dto.Balance != default ? dto.Balance : existingCard.Balance;
               existingCard.IsActive = dto.IsActive != default ? dto.IsActive : existingCard.IsActive;
               existingCard.StateName = dto.StateName ?? existingCard.StateName;
               return existingCard;
          }
     }
}
