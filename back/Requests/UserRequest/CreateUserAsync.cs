using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using BankingAppBackend.Utilities;
using MediatR;

namespace BankingAppBackend.Requests.UserRequest
{
    public class CreateUserAsync : IRequest<(bool status, User user)>
    {
        public SignUpData Data { get; set; }

        public CreateUserAsync(SignUpData data)
        {
            Data = data;
        }
        internal class Handler : IRequestHandler<CreateUserAsync, (bool status, User user)>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;

            public Handler(BankDbContext context, ILogger<Handler> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<(bool status, User user)> Handle(CreateUserAsync request, CancellationToken cancellationToken)
            {
                try
                {
                    User newUser = new User
                    {
                        Id = Guid.NewGuid().ToString(),
                        Email = request.Data.Email,
                        Phone = request.Data.Phone,
                        FullName = request.Data.FullName,
                        PasswordHash = Hasher.HashPassword(request.Data.Email, request.Data.Password),
                        BirthDate = "",
                        Language = "en",
                        Role = "user",
                        SpendingLimit = 1000,
                        TotalBalance = 0,
                        Image = ""
                    };

                    await _context.Users.AddAsync(newUser, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken);

                    return (true, newUser); 
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while creating the user: {ErrorMessage}", ex.Message);
                    return (false, null); 
                }
            }

        }
    }
}
