using Backend_BankingApp.Requests.TransactionsRequest;
using BankingAppBackend.DTO;
using BankingAppBackend.Requests.ApplicationRequest;
using BankingAppBackend.Requests.TransactionsRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppBackend.Controllers
{
     [Route("api/[controller]")]
     [ApiController]
     public class TransactionsController : Controller
     {
          private readonly IMediator _mediator;

          public TransactionsController(IMediator mediator)
          {
               _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
          }
          [HttpPost("add/transaction/buy")]
          public async Task<IActionResult> AddTransactionBuy([FromBody] TransactionBuyDto transaction)
          {
               if (transaction == null)
               {
                    return BadRequest("Invalid transaction data.");
               }

               var result = await _mediator.Send(new AddTransactionB(transaction));
               if (result)
               {
                    return Ok(true);
               }

               return StatusCode(500, "Failed to add transaction.");
          }
          [HttpPost("add/transaction/transfer")]
          public async Task<IActionResult> AddTransactionTransfer([FromBody] TransactionTransferDto transaction)
          {
               if (transaction == null)
               {
                    return BadRequest("Invalid transaction data.");
               }

               var result = await _mediator.Send(new AddTransactionT(transaction));
               if (result)
               {
                    return Ok(true);
               }

               return StatusCode(500, "Failed to add transaction.");
          }
          [HttpPost("add/transaction/supply")]
          public async Task<IActionResult> AddTransactionSupply([FromBody] TransactionSupplyDto transaction)
          {
               if (transaction == null)
               {
                    return BadRequest("Invalid transaction data.");
               }

               var result = await _mediator.Send(new AddTransactionS(transaction));
               if (result)
               {
                    return Ok(true);
               }

               return StatusCode(500, "Failed to add transaction.");
          }

          [HttpGet("{userId}/{type}")]
          public async Task<IActionResult> GetTransactionsByUserId([FromRoute] string userId, [FromRoute] string type)
          {
               if (string.IsNullOrEmpty(userId))
               {
                    return BadRequest("User ID is required.");
               }

               var transactions = await _mediator.Send(new GetTransactionsRequest(userId, type));
               return Ok(transactions.list);
          }
          [HttpGet("notif/{userId}/{type}")]
          public async Task<IActionResult> GetTransactionsNotificationByUserId([FromRoute] string userId, [FromRoute] string type)
          {
               if (string.IsNullOrEmpty(userId))
               {
                    return BadRequest("User ID is required.");
               }

               var transactions = await _mediator.Send(new GetTransactionsNotificationsRequest(userId, type));
               return Ok(transactions.list);
          }
          [HttpGet("notif/{userId}/{type}/number")]
          public async Task<IActionResult> GetTransactionsNotificationByUserIdNumber([FromRoute] string userId, [FromRoute] string type)
          {
               if (string.IsNullOrEmpty(userId))
               {
                    return BadRequest("User ID is required.");
               }

               var transactions = await _mediator.Send(new GetTransactionsNotificationsRequest(userId, type));
               var requests = await _mediator.Send(new GetRequestsBySenderIdAsync(userId));

               var openedRequestsCount = requests.list.Count(r => !r.Closed);
               var openedtransactionsCount = transactions.list.Count(r => !r.IsRead);

               return Ok(openedRequestsCount + openedtransactionsCount);
          }
          [HttpGet("services")]
          public async Task<IActionResult> GetServices()
          {

               var result = await _mediator.Send(new GetServicesRequest());
               return Ok(result.list);
          }
     }
}
