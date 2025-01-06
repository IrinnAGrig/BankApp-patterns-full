namespace Backend_BankingApp.Utilities.State
{
     public static class StateFactory
     {
          private static Dictionary<string, ICreditCardState> _stateCache;

          static StateFactory()
          {
               _stateCache = new Dictionary<string, ICreditCardState>();
               _stateCache["Silver"] = new SilverState();
               _stateCache["Gold"] = new GoldState();
               _stateCache["Platinum"] = new PlatinumState();
          }

          public static ICreditCardState GetState(string stateName)
          {
               return _stateCache.ContainsKey(stateName) ? _stateCache[stateName] : new SilverState();
          }
     }


     public interface ICreditCardState
    {
        decimal GetCreditLimit();
        string GetCardType();
        string GetBenefits();
        decimal GetDiscount();
    }

    public class SilverState : ICreditCardState
    {
        public decimal GetCreditLimit() => 1000;
        public string GetCardType() => "Silver";
        public string GetBenefits() => "Basic benefits";
        public decimal GetDiscount() => 0.05m;
    }

    public class GoldState : ICreditCardState
    {
        public decimal GetCreditLimit() => 5000;
        public string GetCardType() => "Gold";
        public string GetBenefits() => "Premium benefits";
        public decimal GetDiscount() => 0.10m;
    }

    public class PlatinumState : ICreditCardState
    {
        public decimal GetCreditLimit() => 10000;
        public string GetCardType() => "Platinum";
        public string GetBenefits() => "Luxury benefits";
        public decimal GetDiscount() => 0.15m;
    }
}
