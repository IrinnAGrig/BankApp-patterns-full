using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using BankingAppBackend.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Backend_BankingApp.Utilities.Proxy
{
     public interface IUserService
     {
          Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword);
          Task<bool> EditProfileAsync(string userId, User userData);
     }

     public class UserService : IUserService
     {
          private readonly BankDbContext _context;
          private readonly ILogger<UserService> _logger;
  

          public UserService(BankDbContext context, ILogger<UserService> logger)
          {
               _context = context;
               _logger = logger;
          }

          public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
          {
               if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(oldPassword) || string.IsNullOrEmpty(newPassword))
               {
                    _logger.LogWarning("Invalid password change request. User ID or passwords are null or empty.");
                    return false;
               }

               try
               {
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                    {
                         _logger.LogWarning("User not found with ID: {UserId}", userId);
                         return false;
                    }

                    var verificationResult = Hasher.VerifyPassword(user.Email, user.PasswordHash, oldPassword);
                    if (verificationResult == PasswordVerificationResult.Failed)
                    {
                         _logger.LogWarning("Old password verification failed for user: {UserId}", userId);
                         return false;
                    }

                    if (newPassword.Length < 6)
                    {
                         return false;
                    }

                    user.PasswordHash = Hasher.HashPassword(user.Email, newPassword);
                    await _context.SaveChangesAsync();

                    return true;
               }
               catch (Exception ex)
               {
                    _logger.LogError("An error occurred while changing password for user {UserId}: {ErrorMessage}", userId, ex.Message);
                    return false;
               }
          }

          public async Task<bool> EditProfileAsync(string userId, User updatedUser)
          {
               if (string.IsNullOrEmpty(userId) || updatedUser == null)
               {
                    _logger.LogWarning("Invalid profile update request. User ID or user data is null.");
                    return false;
               }

               try
               {
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                    {
                         return false;
                    }

                    user.Email = updatedUser.Email ?? user.Email;
                    user.Image = updatedUser.Image ?? user.Image;
                    user.Phone = updatedUser.Phone ?? user.Phone;
                    user.FullName = updatedUser.FullName ?? user.FullName;
                    user.BirthDate = updatedUser.BirthDate ?? user.BirthDate;
                    user.Language = updatedUser.Language ?? user.Language;
                    user.SpendingLimit = updatedUser.SpendingLimit != 0 ? updatedUser.SpendingLimit : user.SpendingLimit;
                    user.TotalBalance = updatedUser.TotalBalance != 0 ? updatedUser.TotalBalance : user.TotalBalance;

                    await _context.SaveChangesAsync();
                    return true;
               }
               catch (Exception ex)
               {
                    _logger.LogError("An error occurred while updating profile for user {UserId}: {ErrorMessage}", userId, ex.Message);
                    return false;
               }
          }
     }


     public class UserServiceProxy : IUserService
     {
          private readonly IUserService _realUserService;
          private readonly IHttpContextAccessor _httpContextAccessor;
          private readonly ILogger<UserService> _logger;

          public UserServiceProxy(IUserService realUserService, IHttpContextAccessor httpContextAccessor, ILogger<UserService> logger)
          {
               _realUserService = realUserService;
               _httpContextAccessor = httpContextAccessor;
               _logger = logger;
          }

          public async Task<bool> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
          {
               var authenticatedUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? userId;


               if (authenticatedUserId != userId)
               {
                    return false;
               }

               return await _realUserService.ChangePasswordAsync(userId, oldPassword, newPassword);
          }

          public async Task<bool> EditProfileAsync(string userId, User userData)
          {
               var authenticatedUserId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? userId;


               if (authenticatedUserId != userId)
               {
                    return false;
               }

               return await _realUserService.EditProfileAsync(userId, userData);
          }
     }



}