using Backend_BankingApp.Utilities.Decorator;
using Backend_BankingApp.Utilities.Facade;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.CardRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingAppBackend.Requests.ApplicationRequest
{
    public class AddTransactionRequest : IRequest<bool>
    {
        public RequestDto Request { get; set; }

        public AddTransactionRequest(RequestDto req)
        {
            Request = req;
        }
        internal class Handler : IRequestHandler<AddTransactionRequest, bool>
        {
            private readonly BankDbContext _context;
            private readonly ILogger<Handler> _logger;
           private readonly RequestService _requestService;

               public Handler(RequestService requestService, BankDbContext context, ILogger<Handler> logger)
            {
                    _requestService = requestService;
                    _context = context;
                _logger = logger;
            }

            public async Task<bool> Handle(AddTransactionRequest request, CancellationToken cancellationToken)
            {
                try
                {
                         var senderExists = await _requestService.UserExists(request.Request.SenderId, cancellationToken);
                         var receiverExists = await _requestService.UserExists(request.Request.ReceiverId, cancellationToken);

                         if (!senderExists || !receiverExists)
                         {
                              _logger.LogError("Sender or Receiver not found in the Users table.");
                              return false;
                         }

                         var req = new RequestModel
                         {
                              Id = Guid.NewGuid().ToString(),
                              Name = request.Request.Name,
                              ReceiverId = request.Request.ReceiverId,
                              SenderId = request.Request.SenderId,
                              Amount = request.Request.Amount,
                              Valute = request.Request.Valute,
                              DueDate = request.Request.DueDate,
                              Phone = request.Request.Phone,
                              Email = request.Request.Email,
                              DateSent = DateTime.Now.ToString(),
                              Closed = false,
                              Status = request.Request.Status,
                              DateClosed = "",
                              IsRead = false
                         };

                         var result = await _requestService.AddRequestAsync(req, cancellationToken);
                         return result;
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error occurred while adding the transaction: {ErrorMessage}", ex.Message);
                    return false; 
                }
            }
        }

    }
}
