using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HWA.GARDEN.EventService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            Requires.NotNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        // GET api/v1/[controller]/items[?startDate=...&endDate=...]
        [HttpGet]
        [Route("items")]
        //[ProducesResponseType(typeof(Event), StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IAsyncEnumerable<Event> GetEventsAsync([FromQuery]string startDate, [FromQuery] string endDate
            , CancellationToken cancellationToken)
        {
            DateOnly start, end;
            if(!DateOnly.TryParse(startDate, out start))
            {
                throw new InvalidOperationException($"The wrong value has been provided for \"{nameof(startDate)}\" parameter.");
            }
            if (!DateOnly.TryParse(endDate, out end))
            {
                throw new InvalidOperationException($"The wrong value has been provided for \"{nameof(endDate)}\" parameter.");
            }

            return _mediator.CreateStream(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }, cancellationToken);
        }
    }
}
