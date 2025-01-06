using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.UserRequest
{
    public class FindByEmailAsync : IRequest<User>
    {
        public string Email { get; set; }
        public FindByEmailAsync(string email)
        {
            Email = email;
        }
        internal class GetHandler : IRequestHandler<FindByEmailAsync, User>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<User> Handle(FindByEmailAsync request, CancellationToken cancellationToken)
            {
               

                try
                {
                    User user = await _context.Users
                        .FirstOrDefaultAsync(g => EF.Functions.Like(g.Email, request.Email), cancellationToken);

                    if (user != null)
                    {
                        return user;
                    }
                    else
                    {
                        _logger.LogWarning("No user found with email: {Email}", request.Email);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while finding user by email: {Email}. Error: {Message}", request.Email, ex.Message);
                    return null;
                }
            }

        }
    }
}
