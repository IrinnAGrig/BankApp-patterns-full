using Backend_BankingApp.Utilities;
using BankingAppBackend.Model;

namespace Backend_BankingApp.Model
{
    //public abstract class Transaction
    //{
    //    public required string Id { get; set; }
    //    public required string SenderId { get; set; }
    //    public User? Sender { get; set; }
    //    public decimal Sum { get; set; }
    //    public required string Valute { get; set; }
    //    public string Date { get; set; }
    //    public required string CardId { get; set; }
    //    public Card? Card { get; set; }
    //    public bool IsRead { get; set; }

    //    public Transaction(string senderId, decimal sum, string valute, string cardId)
    //    {
    //        Id = IdGenerator.GenerateId(senderId, cardId); 
    //        SenderId = senderId; 
    //        Sum = sum;
    //        Valute = valute; 
    //        CardId = cardId; 
    //        Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    //    }
    //}

    public class TransactionBuy
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public User? Sender { get; set; }
        public decimal Sum { get; set; }
        public string Valute { get; set; }
        public string Date { get; set; }
        public string CardId { get; set; }
        public Card? Card { get; set; }
        public bool IsRead { get; set; }
        public string ServiceId { get; set; }
        public Service? Service { get; set; }
          public decimal RemainAmount { get; set; }
          public TransactionBuy(string senderId, decimal sum, string valute, string serviceId, string cardId)
        {
            if (string.IsNullOrWhiteSpace(senderId))
                throw new ArgumentNullException(nameof(senderId), "SenderId nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(valute))
                throw new ArgumentNullException(nameof(valute), "Valute nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(serviceId))
                throw new ArgumentNullException(nameof(serviceId), "ServiceId nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(cardId))
                throw new ArgumentNullException(nameof(cardId), "CardId nu poate fi null sau gol.");
            

            Id = IdGenerator.GenerateId(senderId, cardId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")); // Generarea ID-ului unic
            SenderId = senderId;
            Sum = sum;
            Valute = valute;
            ServiceId = serviceId;
            CardId = cardId;
            IsRead = false;
            Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
        }

    }

    public class TransactionSupply
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public User? Sender { get; set; }
        public decimal Sum { get; set; }
        public string Valute { get; set; }
        public string Date { get; set; }
        public string CardId { get; set; }
        public Card? Card { get; set; }
        public bool IsRead { get; set; }
          public decimal RemainAmount { get; set; }

          public TransactionSupply() { }

          public TransactionSupply(string senderId, decimal sum, string valute, string cardId)
        {
            if (string.IsNullOrWhiteSpace(senderId))
                throw new ArgumentNullException(nameof(senderId), "SenderId nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(valute))
                throw new ArgumentNullException(nameof(valute), "Valute nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(cardId))
                throw new ArgumentNullException(nameof(cardId), "CardId nu poate fi null sau gol.");

               Id = IdGenerator.GenerateId(senderId, cardId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
               SenderId = senderId;
            Sum = sum;
            Valute = valute;
            CardId = cardId;
               IsRead = false;
               Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }


    public class TransactionTransfer
    {
        public string Id { get; set; }
        public string SenderId { get; set; }
        public User? Sender { get; set; }
        public decimal Sum { get; set; }
        public string Valute { get; set; }
        public string Date { get; set; }
        public string CardId { get; set; }
        public Card? Card { get; set; }
        public bool IsRead { get; set; }
        public string ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public string ReceiverCardId { get; set; }
          public decimal RemainAmount { get; set; }

          public TransactionTransfer() { }

          public TransactionTransfer(string senderId, string receiverId, string receiverCardId, decimal sum, string valute, string cardId)
        {
            if (string.IsNullOrWhiteSpace(senderId))
                throw new ArgumentNullException(nameof(senderId), "SenderId nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(receiverId))
                throw new ArgumentNullException(nameof(receiverId), "ReceiverId nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(receiverCardId))
                throw new ArgumentNullException(nameof(receiverCardId), "ReceiverCardNumber nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(valute))
                throw new ArgumentNullException(nameof(valute), "Valute nu poate fi null sau gol.");
            if (string.IsNullOrWhiteSpace(cardId))
                throw new ArgumentNullException(nameof(cardId), "CardId nu poate fi null sau gol.");

               Id = IdGenerator.GenerateId(senderId, cardId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
               SenderId = senderId;
            ReceiverId = receiverId;
            ReceiverCardId = receiverCardId;
            Sum = sum;
            Valute = valute;
            CardId = cardId;
          IsRead = false;
          Date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }

}
