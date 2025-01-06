using Backend_BankingApp.Model;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO;
using BankingAppBackend.Model;

namespace Backend_BankingApp.Utilities.Adapter
{
    public interface ITransactionAdapter
    {
        TransactionShow Adapt(object transaction);
        Notification AdaptNotif(object transaction);
     }
    public class TransactionAdapter : ITransactionAdapter
    {
        private readonly BankDbContext _context;

        public TransactionAdapter(BankDbContext context)
        {
            _context = context;
        }

        public TransactionShow Adapt(object transaction)
        {
            return transaction switch
            {
                TransactionBuy buyTransaction => AdaptBuyTransaction(buyTransaction),
                TransactionSupply supplyTransaction => AdaptSupplyTransaction(supplyTransaction),
                TransactionTransfer transferTransaction => AdaptTransferTransaction(transferTransaction),
                _ => throw new ArgumentException("Unsupported transaction type.")
            };
        }
          public Notification AdaptNotif(object transaction)
          {
               var selectedCard = transaction switch
               {
                    TransactionBuy buyTransaction => _context.Cards.FirstOrDefault(c => c.Id == buyTransaction.CardId),
                    TransactionSupply supplyTransaction => _context.Cards.FirstOrDefault(c => c.Id == supplyTransaction.CardId),
                    TransactionTransfer transferTransaction => _context.Cards.FirstOrDefault(c => c.Id == transferTransaction.CardId),
                    RequestModel requestTransaction => null,
                    _ => throw new ArgumentException("Unsupported transaction type.")
               };
               return transaction switch
               {
                    TransactionBuy buyTransaction => AdaptNotifBuyTransaction(buyTransaction, selectedCard),
                    TransactionSupply supplyTransaction => AdaptNotifSupplyTransaction(supplyTransaction, selectedCard),
                    TransactionTransfer transferTransaction => AdaptNotifTransferTransaction(transferTransaction, selectedCard),
                    RequestModel requestTransaction => AdaptNotifTransferRequest(requestTransaction),
                    _ => throw new ArgumentException("Unsupported transaction type.")
               };
          }

          private TransactionShow AdaptBuyTransaction(TransactionBuy transaction)
        {
            var service = _context.Services.FirstOrDefault(s => s.IdService == transaction.ServiceId);

            return new TransactionShow
            {
                Id = transaction.Id,
                Type = "Buy",
                Title = service?.Title ?? "Unknown Service",
                Subtitle = service?.Subtitle ?? "No Details",
                Sum = transaction.Sum,
                Valute = transaction.Valute,
                Emblem = service.Emblem,
                Date = transaction.Date
            };
        }

        private TransactionShow AdaptSupplyTransaction(TransactionSupply transaction)
        {
            return new TransactionShow
            {
                Id = transaction.Id,
                Type = "Supply",
                Title = "Supply Transaction",
                Subtitle = "Funds added to account",
                Sum = transaction.Sum,
                Valute = transaction.Valute,
                Emblem = "",
                Date = transaction.Date
            };
        }

        private TransactionShow AdaptTransferTransaction(TransactionTransfer transaction)
        {
            var receiverName = _context.Users.FirstOrDefault(u => u.Id == transaction.ReceiverId)?.FullName ?? "Unknown";

            return new TransactionShow
            {
                Id = transaction.Id,
                Type = "Transfer",
                Title = $"Transfer to {receiverName}",
                Subtitle = $"Card: {transaction.ReceiverCardId}",
                Sum = transaction.Sum,
                Valute = transaction.Valute,
                Emblem = "",
                Date = transaction.Date
            };
        }
          private Notification AdaptNotifBuyTransaction(TransactionBuy transaction, Card card)
          {
               var service = _context.Services.FirstOrDefault(s => s.IdService == transaction.ServiceId);
               return new Notification
               {
                    Id = transaction.Id,
                    Type = "Buy",
                    Timestamp = transaction.Date, 
                    Message = $"Payment made with card {MaskCardNumber(card.CardNumber)}, amount: {transaction.Sum} {transaction.Valute}.",
                    AvailableBalance = transaction.RemainAmount,
                    Emblem = "",
                    IsRead = transaction.IsRead
               };
          }
          public static string MaskCardNumber(string cardNumber)
          {
               if (string.IsNullOrEmpty(cardNumber) || cardNumber.Length < 4)
               {
                    throw new ArgumentException("Numărul cardului trebuie să aibă cel puțin 4 caractere.");
               }

               string lastFourDigits = cardNumber.Substring(cardNumber.Length - 4);

               string maskedPart = new string('*', cardNumber.Length - 4);
               return maskedPart + lastFourDigits;
          }
          private Notification AdaptNotifSupplyTransaction(TransactionSupply transaction, Card card)
          {
               return new Notification
               {
                    Id = transaction.Id,
                    Type = "Supply",
                    Timestamp = transaction.Date, 
                    Message = $"Card {MaskCardNumber(card.CardNumber)} credited with amount: {transaction.Sum} {transaction.Valute}.",
                    AvailableBalance = transaction.RemainAmount,
                    Emblem = "",
                    IsRead = transaction.IsRead
               };
          }

          private Notification AdaptNotifTransferTransaction(TransactionTransfer transaction, Card card)
          {
               var receiverName = _context.Users.FirstOrDefault(u => u.Id == transaction.ReceiverId)?.FullName ?? "Unknown";

               return new Notification
               {
                    Id = transaction.Id,
                    Type = "Transfer",
                    Timestamp = transaction.Date, 
                    Message = $"Transfer P2P from person ${receiverName} {MaskCardNumber(card.CardNumber)} with amount: {transaction.Sum} {transaction.Valute}.",
                    AvailableBalance = transaction.RemainAmount,
                    Emblem = "",
                    IsRead = transaction.IsRead
               };
          }
          private Notification AdaptNotifTransferRequest(RequestModel request)
          {

               return new Notification
               {
                    Id = request.Id,
                    Type = "Request",
                    Timestamp = request.DateClosed, 
                    Message = $"Received from person {request.Name} amount: {request.Amount} {request.Valute}.",
                    AvailableBalance = request.RemainAmount,
                    Emblem = "",
                    IsRead = request.IsRead
               };
          }
     }
}
