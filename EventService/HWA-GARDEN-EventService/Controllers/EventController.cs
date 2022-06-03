using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.EventService.Models;
using HWA.GARDEN.Utilities.Extensions;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace HWA.GARDEN.EventService.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class EventController : Controller
    {
        private readonly IMediator _mediator;

        public EventController(IMediator mediator)
        {
            Requires.NotNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        // GET api/v1/events[?startDate=...&endDate=...]
        [HttpGet("events")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IAsyncEnumerable<Event> GetEventsAsync([FromQuery] GetEventsByPeriod query
            , CancellationToken cancellationToken)
        {
            return _mediator.CreateStream(
                new GetEventListByPeriodQuery
                {
                    StartDate = query.StartDate.ToDateOnly(),
                    EndDate = query.EndDate.ToDateOnly()
                }, cancellationToken);
        }
    }
}
