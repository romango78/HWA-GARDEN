using AutoMapper;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace HWA.GARDEN.CalendarService.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class CalendarController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator, IMapper mapper)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(mapper, nameof(mapper));

            _mediator = mediator;
            _mapper = mapper;
        }

        // GET api/v1/calendars[?year=...]
        [HttpGet("calendars")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<Calendar>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IAsyncEnumerable<Calendar> GetCalendarListAsync([Required] int year, CancellationToken cancellationToken)
        {
            return _mediator.CreateStream(_mapper.Map<CalendarListQuery>(year), cancellationToken);
        }
    }
}