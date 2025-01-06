using Backend_BankingApp.Utilities.State;
using BankingAppBackend.Model;

namespace Backend_BankingApp.Utilities.Strategy
{
    public interface IPaymentStrategy
    {
        decimal ApplyPayment(Card card, decimal amount, string service);
    }
    public class DebitCardPaymentStrategy : IPaymentStrategy
    {
        public decimal ApplyPayment(Card card, decimal amount, string serviceId)
        {
            decimal discount = 0;

            if (amount > 1000)
            {
                discount = amount * 0.05m; 
            }

            decimal finalAmount = amount - discount;

            if (serviceId == "1" || serviceId == "2")
            {
                finalAmount += 2; 
            }

            return finalAmount;
        }
    }
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        private readonly ICreditCardState _cardState;

        public CreditCardPaymentStrategy(ICreditCardState cardState)
        {
            _cardState = cardState;
        }

        public decimal ApplyPayment(Card card, decimal amount, string service = "0")
        {
            decimal discount = amount * _cardState.GetDiscount(); 

            decimal finalAmount = amount - discount;

            return finalAmount;
        }
    }


}

