using Backend_BankingApp.Model;

namespace BankingAppBackend.Model
{
    public class Transactions
    {
        public decimal Sum { get; set; }
        public string Valute { get; set; }
        public string Emblem { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
    }

    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class SignUpData
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
    }

     public class User
     {
          public required string Id { get; set; }
          public string Email { get; set; }
          public string Image { get; set; }
          public string Phone { get; set; }
          public string FullName { get; set; }
          public string PasswordHash { get; set; }
          public string BirthDate { get; set; }
          public string Role { get; set; }
          public string Language { get; set; }
          public decimal SpendingLimit { get; set; }
          public decimal TotalBalance { get; set; }
     }

     public class ErrorInfo
    {
        public bool HasError { get; set; }
        public string Error { get; set; }
    }

    public class RecentUser
    {
        public required string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string CardNumber { get; set; }
    }

     public class RecentUserSimple
     {
          public string IdUser { get; set; }
          public string CardNumber { get; set; }
     }
     public class ChangePasswordData
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

}
