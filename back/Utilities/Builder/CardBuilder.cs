using Backend_BankingApp.Utilities.State;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;


namespace Backend_BankingApp.Utilities.Builder
{
     public interface ICardBuilder
     {
          void SetCardInfo(CardDTO cardDto); 
          void SetGeneralInfo();            
          void SetBalanceState(CardDTO cardDto);   
          Card GetResult();
     }

     public class DebitCardBuilder : ICardBuilder
     {
          public Card _card = new Card();

          public void SetCardInfo(CardDTO cardDto)
          {
               _card.OwnerId = cardDto.OwnerId;
               _card.CardNumber = cardDto.CardNumber;
               _card.NameHolder = cardDto.NameHolder;
               _card.ExpiryDate = cardDto.ExpiryDate;
               _card.Cvv = cardDto.Cvv;
               _card.Valute = cardDto.Valute;
          }

          public void SetGeneralInfo()
          {
               _card.IsActive = true;
               _card.IssuedDate = DateTime.UtcNow;
               _card.TypeCard = "Debit";
          }

          public void SetBalanceState(CardDTO cardDto)
          {
               _card.StateName = cardDto.Type;
               _card.Balance = 0m;
          }

          public Card GetResult()
          {
               return _card;
          }
     }

     public class CreditCardBuilder : ICardBuilder
     {
          private Card _card = new Card();

          public void SetCardInfo(CardDTO cardDto)
          {
               _card.OwnerId = cardDto.OwnerId;
               _card.CardNumber = cardDto.CardNumber;
               _card.NameHolder = cardDto.NameHolder;
               _card.ExpiryDate = cardDto.ExpiryDate;
               _card.Cvv = cardDto.Cvv;
               _card.Valute = cardDto.Valute;
          }

          public void SetGeneralInfo()
          {
               _card.IsActive = true;
               _card.IssuedDate = DateTime.UtcNow;
               _card.TypeCard = "Credit";
          }

          public void SetBalanceState(CardDTO cardDto)
          {
               _card.StateName = cardDto.Type;
               var state = StateFactory.GetState(cardDto.Type);
               _card.Balance = state.GetCreditLimit();
          }


          public Card GetResult()
          {
               return _card;
          }
     }

     public class CardBuildDirector
     {
          private readonly ICardBuilder _builder;

          public CardBuildDirector(ICardBuilder builder)
          {
               _builder = builder;
          }

          public void Construct(CardDTO cardDto)
          {
               _builder.SetCardInfo(cardDto); 
               _builder.SetGeneralInfo();   
               _builder.SetBalanceState(cardDto);
          }

          public Card GetResult()
          {
               return _builder.GetResult();
          }
     }
}
