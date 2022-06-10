using AutoMapper;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.EventService.Models;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace HWA.GARDEN.EventService.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class EventController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public EventController(IMediator mediator, IMapper mapper)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(mapper, nameof(mapper));

            _mediator = mediator;
            _mapper = mapper;
        }

        // GET api/v1/events[?startDate=...&endDate=...]
        [HttpGet("events")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IAsyncEnumerable<Event> GetEventsAsync([FromQuery] GetEventsByPeriodModel query
            , CancellationToken cancellationToken)
        {
            return _mediator.CreateStream(_mapper.Map<GetEventListByPeriodQuery>(query), cancellationToken);
        }

        [HttpPost("events")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<Event> CreateEventAsync(EventModel model, CancellationToken cancellationToken)
        {
            return _mediator.Send(_mapper.Map<CreateEventRequest>(model), cancellationToken);
        }
    }
}
