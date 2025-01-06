using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using BankingAppBackend.Utilities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace BankingAppBackend.Requests.UserRequest
{
    public class UpdateAsync : IRequest<bool>
    {
        public string Id { get; set; } 
        public User User { get; set; }

        public UpdateAsync(string id, User user)
        {
            Id = id;
            User = user;
        }
        internal class Handler : IRequestHandler<UpdateAsync, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(BankDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(UpdateAsync request, CancellationToken cancellationToken)
            {

                try
                {
                    var user = await _context.Users.FindAsync(request.Id);
                    if (user!= null)
                    {
                         user.Email = request.User.Email;
                         user.Image = request.User.Image;
                         user.Phone = request.User.Phone;
                         user.FullName = request.User.FullName;
                         user.PasswordHash = request.User.PasswordHash;
                         user.BirthDate = request.User.BirthDate;
                         user.Language = request.User.Language;
                         user.SpendingLimit = request.User.SpendingLimit;
                         user.TotalBalance = request.User.TotalBalance;
                              await _context.SaveChangesAsync(cancellationToken);
                        return true;
                    }
                    else
                    {
                        _logger.LogWarning("Old password verification failed for user: {UserId}", request.Id);
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("An error occurred while changing password for user {UserId}: {ErrorMessage}", request.Id, ex.Message);
                    return false;
                }
            }
        }
    }
}
