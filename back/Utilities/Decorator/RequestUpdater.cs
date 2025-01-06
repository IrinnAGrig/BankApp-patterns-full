using BankingAppBackend.DAL;
using BankingAppBackend.Model;
using System.Text.RegularExpressions;

namespace Backend_BankingApp.Utilities.Decorator
{
     public interface IRequestUpdater
     {
          Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken);
     }

     public class BaseRequestUpdater : IRequestUpdater
     {
          private readonly BankDbContext _context;

          public BaseRequestUpdater(BankDbContext context)
          {
               _context = context;
          }

          public async Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {
               var existingRequest = await _context.RequestsMoney.FindAsync(new object[] { request.Id }, cancellationToken);
               if (existingRequest == null)
               {
                    return false;
               }

               existingRequest.Closed = true;
               existingRequest.Status = request.Status;
               await _context.SaveChangesAsync(cancellationToken);
               return true;
          }
     }

     public abstract class RequestUpdaterDecorator : IRequestUpdater
     {
          protected IRequestUpdater _inner;

          public RequestUpdaterDecorator(IRequestUpdater inner)
          {
               _inner = inner;
          }

          public virtual async Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {
               return await _inner.UpdateRequestAsync(request, cancellationToken);
          }
     }

     public class AmountRangeRequestUpdater : RequestUpdaterDecorator
     {
          private readonly decimal _minAmount;
          private readonly decimal _maxAmount;

          public AmountRangeRequestUpdater(IRequestUpdater inner, decimal minAmount = 10, decimal maxAmount = 10000)
              : base(inner)
          {
               _minAmount = minAmount;
               _maxAmount = maxAmount;
          }

          public override async Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {
               if (request.Amount < _minAmount || request.Amount > _maxAmount)
               {
                    throw new ArgumentException($"Amount must be between {_minAmount} and {_maxAmount}. Current value: {request.Amount}");
               }

               return await base.UpdateRequestAsync(request, cancellationToken);
          }
     }
     public class EmailValidationRequestUpdater : RequestUpdaterDecorator
     {
          public EmailValidationRequestUpdater(IRequestUpdater inner) : base(inner)
          {
          }

          public override async Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {

               if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
               {
                    throw new ArgumentException($"Invalid email address format: {request.Email}");
               }

               return await base.UpdateRequestAsync(request, cancellationToken);
          }

          private bool IsValidEmail(string email)
          {
               var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
               return emailRegex.IsMatch(email);
          }
     }

     public class ValidationRequestUpdater : RequestUpdaterDecorator
     {
          public ValidationRequestUpdater(IRequestUpdater inner) : base(inner)
          {
          }

          public override async Task<bool> UpdateRequestAsync(RequestModel request, CancellationToken cancellationToken)
          {
               if (request.Amount <= 0)
               {
                    throw new ArgumentException("Amount must be positive.");
               }
               if (DateTime.TryParse(request.DueDate, out var dueDate))
               {
                    if (dueDate < DateTime.Now)
                    {
                         throw new InvalidOperationException($"DueDate {request.DueDate} is expired and cannot be updated.");
                    }
               }
               else
               {
                    throw new ArgumentException($"Invalid DueDate format: {request.DueDate}");
               }
               return await base.UpdateRequestAsync(request, cancellationToken);
          }
     }
}
