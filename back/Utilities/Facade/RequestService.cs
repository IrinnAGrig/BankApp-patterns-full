using Backend_BankingApp.Utilities.Decorator;
using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using Microsoft.EntityFrameworkCore;

namespace Backend_BankingApp.Utilities.Facade
{
     public class RequestService
     {
          private readonly BankDbContext _context;
          private readonly ILogger<RequestService> _logger;

          public RequestService(BankDbContext context, ILogger<RequestService> logger)
          {
               _context = context;
               _logger = logger;
          }

          public async Task<bool> UserExists(string userId, CancellationToken cancellationToken)
          {
               return await _context.Users.AnyAsync(u => u.Id == userId, cancellationToken);
          }

          public async Task<bool> AddRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {
               try
               {
                    IRequestUpdater requestUpdater = new BaseRequestUpdater(_context);
                    requestUpdater = new ValidationRequestUpdater(requestUpdater);
                    requestUpdater = new AmountRangeRequestUpdater(requestUpdater, 10, 10000); 
                    requestUpdater = new EmailValidationRequestUpdater(requestUpdater);

                    await requestUpdater.UpdateRequestAsync(request, cancellationToken);

                    await _context.RequestsMoney.AddAsync(request, cancellationToken);
                    var result = await _context.SaveChangesAsync(cancellationToken);
                    return result > 0;
               }
               catch (Exception ex)
               {
                    _logger.LogError("Error while adding the request: {Message}", ex.Message);
                    return false;
               }
          }
     }


}
