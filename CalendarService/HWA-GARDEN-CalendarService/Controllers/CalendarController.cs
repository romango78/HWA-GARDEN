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
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            Requires.NotNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        // GET api/v1/calendars[?year=...]
        [HttpGet("calendars")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IAsyncEnumerable<Calendar>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IAsyncEnumerable<Calendar> GetCalendarListAsync([Required] int year, CancellationToken cancellationToken)
        {
            return _mediator.CreateStream(new CalendarListQuery { Year = year }, cancellationToken);
        }
    }
}