using BankingAppBackend.Model;

namespace BankingAppBackend.DTO
{
     public interface TransactionDto
     {
          public string SenderId { get; set; }
          public decimal Sum { get; set; }
          public string Valute { get; set; }
          public string CardId { get; set; }
     }

     public class TransactionBuyDto : TransactionDto
     {
          public string ServiceId { get; set; }
          public string SenderId { get; set; }
          public decimal Sum { get; set; }
          public string Valute { get; set; }
          public string CardId { get; set; }
     }

     public class TransactionSupplyDto : TransactionDto
     {
          public string SenderId { get; set; }
          public decimal Sum { get; set; }
          public string Valute { get; set; }
          public string CardId { get; set; }
     }

     public class TransactionTransferDto : TransactionDto
     {
          public string SenderId { get; set; }
          public decimal Sum { get; set; }
          public string Valute { get; set; }
          public string CardId { get; set; }
          public string ReceiverId { get; set; }
          public string ReceiverCardId { get; set; }
     }


     public class TransactionShow
     {
          public string Id { get; set; } 
          public string Type { get; set; }
          public decimal Sum { get; set; } 
          public string Valute { get; set; } 
          public string Date { get; set; }
          public string Title { get; set; }
          public string Subtitle { get; set; } 
          public string Emblem { get; set; } 
     }
     public class Notification
     {
          public string Id { get; set; }
          public string Type { get; set; }
          public string Timestamp { get; set; }
          public string Message { get; set; }
          public decimal AvailableBalance { get; set; }
          public string Emblem { get; set; } 
          public bool IsRead { get; set; }
     }

}

