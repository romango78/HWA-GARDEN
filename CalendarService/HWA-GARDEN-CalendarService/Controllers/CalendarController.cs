using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HWA.GARDEN.CalendarService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CalendarController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CalendarController(IMediator mediator)
        {
            Requires.NotNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        // GET api/v1/[controller]/items[?year=...]
        [HttpGet]
        [Route("items")]
        public IAsyncEnumerable<Calendar> GetCalendarListAsync([FromQuery] int year, CancellationToken token)
        {
            return _mediator.CreateStream(new CalendarListQuery { Year = year }, token);
        }
    }
}