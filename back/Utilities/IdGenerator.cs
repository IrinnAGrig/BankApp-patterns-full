using System.Security.Cryptography;
using System.Text;

namespace Backend_BankingApp.Utilities
{
     public static class IdGenerator
     {
          public static string GenerateId(params string[] data)
          {
               using (var sha256 = SHA256.Create())
               {
                    var rawData = string.Join("-", data);
                    var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                    return Convert.ToBase64String(bytes)
                        .Replace("/", "")
                        .Replace("+", "")
                        .TrimEnd('=');
               }
          }
     }
}
