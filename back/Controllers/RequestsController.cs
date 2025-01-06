using BankingAppBackend.DTO.Requests;
using BankingAppBackend.Requests.ApplicationRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : Controller
    {
        private readonly IMediator _mediator;

        public RequestsController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction([FromBody] RequestDto request)
        {

            var result = await _mediator.Send( new AddTransactionRequest(request));
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRequest([FromRoute]  string id, [FromBody] RequestUpdateModel request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(new UpdateRequestAsync(request));
            return Ok(result);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetRequestsByIdUser([FromRoute] string userId)
        {
            var (status, requestList) = await _mediator.Send(new GetRequestsBySenderIdAsync(userId));

            if (!status)
            {
                return BadRequest("Unable to retrieve requests.");
            }

            return Ok(requestList.AsEnumerable().Reverse());
        }


        [HttpGet("user/{userId}/opened")]
        public async Task<IActionResult> GetNumberOpenedRequests([FromRoute] string userId)
        {
            var (status, requestList) = await _mediator.Send(new GetRequestsBySenderIdAsync(userId));

            if (!status)
            {
                return BadRequest("Unable to retrieve requests.");
            }

            var openedRequestsCount = requestList.Count(r => !r.Closed);

            return Ok(openedRequestsCount);
        }

    }
}
