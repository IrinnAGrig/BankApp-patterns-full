using Backend_BankingApp.Utilities.State;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingAppBackend.Model
{
     public interface IPrototype<T>
     {
          T Clone();
     }
     public class Card : IPrototype<Card>
     {
          public string Id { get; set; }
          public string TypeCard { get; set; } 
          public string Type { get; set; } 
          public string Network { get; set; } 
          public string OwnerId { get; set; }
          public User User { get; set; }
          public string CardNumber { get; set; }
          public string NameHolder { get; set; }
          public string ExpiryDate { get; set; }
          public string Cvv { get; set; }
          public string Valute { get; set; }
          public decimal Balance { get; set; }
          public DateTime IssuedDate { get; set; }
          public bool IsActive { get; set; }

         
          public string StateName { get; set; }
          [NotMapped]
          public ICreditCardState State { get; set; }

          public void SetState(ICreditCardState state)
          {
               State = state;
          }

          public decimal GetDiscount()
          {
               return State.GetDiscount();
          }

          public Card Clone()
          {
               return (Card)this.MemberwiseClone(); 
          }
     }



}
