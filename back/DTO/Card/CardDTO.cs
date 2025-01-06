namespace BankingAppBackend.DTO.Card
{
     public class CardDTO
     {
          public string OwnerId { get; set; }
          public string CardNumber { get; set; }
          public string NameHolder { get; set; }
          public string ExpiryDate { get; set; } 
          public string Cvv { get; set; }
          public string Type { get; set; }
          public string TypeCard { get; set; }
          public string Network { get; set; }
          public decimal Balance { get; set; } 
          public string Valute { get; set; }
     }

     public class CardUpdateDto
     {
          public string Id { get; set; }
          public string TypeCard { get; set; }
          public string Type { get; set; }
          public string Network { get; set; }
          public string OwnerId { get; set; }
          public string CardNumber { get; set; }
          public string NameHolder { get; set; }
          public string ExpiryDate { get; set; }
          public string Cvv { get; set; }
          public string Valute { get; set; }
          public decimal Balance { get; set; }
          public DateTime IssuedDate { get; set; }
          public bool IsActive { get; set; }
          public string StateName { get; set; }
     }

}
