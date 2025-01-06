using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BankingAppBackend.Requests.UserRequest
{
    public class FindUserByIdAsync : IRequest<User>
    {
        public string Id { get; set; }
        public FindUserByIdAsync(string id)
        {
            Id = id;
        }
        internal class GetHandler : IRequestHandler<FindUserByIdAsync, User>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<GetHandler> _logger;

            public GetHandler(BankDbContext context, ILogger<GetHandler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<User> Handle(FindUserByIdAsync request, CancellationToken cancellationToken)
            {


                try
                {
                    User user = await _context.Users
                        .FirstOrDefaultAsync(g => EF.Functions.Like(g.Id, request.Id), cancellationToken);

                    if (user != null)
                    {
                        return user;
                    }
                    else
                    {
                        _logger.LogWarning("No user found with email: {Email}", request.Id);
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while finding user by email: {Email}. Error: {Message}", ex.Message);
                    return null;
                }
            }

        }
    }
}
