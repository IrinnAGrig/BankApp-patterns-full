using BankingAppBackend.Model;
using Microsoft.AspNetCore.Identity;

namespace BankingAppBackend.Utilities
{

    public static class Hasher
    {
        private static readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();

        public static string HashPassword(string email, string password)
        {
            return _passwordHasher.HashPassword(("el135" + email), password);
        }

        public static PasswordVerificationResult VerifyPassword(string email, string hashedPassword,  string password)
        {
            return _passwordHasher.VerifyHashedPassword(("el135" + email), hashedPassword, password);
        }
    }
}
