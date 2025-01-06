using Backend_BankingApp.Utilities.Adapter;
using Backend_BankingApp.Utilities.Observer;
using BankingAppBackend.DAL;
using BankingAppBackend.DTO.Card;
using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.CardRequest;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BankingAppBackend.Requests.ApplicationRequest
{
    public class UpdateRequestAsync : IRequest<bool>
    {
        public RequestUpdateModel Request { get; set; }

        public UpdateRequestAsync(RequestUpdateModel Req)
        {
            Request = Req;
        }
        internal class Handler : IRequestHandler<UpdateRequestAsync, bool>
        {
            private readonly BankDbContext _context;
               private readonly WebSocketService _webSocketService;

               public Handler(BankDbContext context, WebSocketService webSocketService)
            {
                _context = context;
                _webSocketService = webSocketService;
            }

            public async Task<bool> Handle(UpdateRequestAsync request, CancellationToken cancellationToken)
            {
                try
                {
                    var req = await _context.RequestsMoney.FindAsync(request.Request.Id);
                    
                    if (req != null)
                    {
                         req.Closed = true;
                         req.Status = request.Request.Status;
                         req.DateClosed = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); ;
                              req.IsRead = request.Request.IsRead;
                         }
                    else
                    {
                        return false;
                    }
                         List<Card> cards = new List<Card>();

                         cards = await _context.Cards
                                 .Where(g => EF.Functions.Like(g.OwnerId, request.Request.SenderId))
                                 .ToListAsync();
                                 
                         if (request.Request.Status == 'A')
                         {
                              req.RemainAmount = cards[0].Balance + request.Request.Amount;
                              cards[0].Balance = req.RemainAmount;
                              var result = await _context.SaveChangesAsync();

                              if (result > 0)
                              {
                                   var adapter = new TransactionAdapter(_context);
                                   var notification = adapter.AdaptNotif(req);

                                   var messageJson = JsonSerializer.Serialize(notification);
                                   await _webSocketService.SendMessageToClient(request.Request.ReceiverId, messageJson);

                                   return true;
                              }
                         }else if(request.Request.Status == 'D'){
                              var result = await _context.SaveChangesAsync();
                              if (result > 0)
                              {
                                   return true;
                              }
                         }

                         return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
     }
}
