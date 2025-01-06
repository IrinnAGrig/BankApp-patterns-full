using BankingAppBackend.DTO.Card;
using BankingAppBackend.Model;
using BankingAppBackend.Requests.CardRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankingAppBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : Controller
    {
        private readonly IMediator _mediator;

        public CardController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> AddCard([FromBody] CardDTO card)
        {
            if (card == null)
            {
                return BadRequest("Card is null");
            }

            var result = await _mediator.Send(new AddCardRequest(card));
            if (result)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> UpdateCard([FromRoute] string id, [FromBody] CardUpdateDto card)
        {
            if (id != card.Id)
            {
                return BadRequest("Card ID mismatch");
            }

            var result = await _mediator.Send(new UpdateCardCommand(id, card));
            if (result)
            {
                return Ok(true);
            }
            return BadRequest(false);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardsByOwnerId([FromRoute] string id)
        {
            var cards = await _mediator.Send(new GetCardsByOwnerIdQuery(id));
            return Ok(cards.list);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindCardByNumberAndName([FromQuery] string cardNumber)
        {
            var card = await _mediator.Send(new FindCardByNumberRequest(cardNumber));
            if (card.card != null)
            {
                return Ok(card);
            }
            return NotFound();
        }
    }
}
