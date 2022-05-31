using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.Utilities.Validation;
using MassTransit;
using MediatR;

namespace HWA.GARDEN.CalendarService.Comsumers
{
    public class GetCalendarListConsumer : IConsumer<GetCalendarList>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<GetCalendarListConsumer> _logger;

        public GetCalendarListConsumer(IMediator mediator, ILogger<GetCalendarListConsumer> logger)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(logger, nameof(logger));

            _mediator = mediator;
            _logger = logger;
        }

        async Task IConsumer<GetCalendarList>.Consume(ConsumeContext<GetCalendarList> context)
        {            
            try
            {
                IList<Calendar> result =
                    await _mediator.CreateStream(new CalendarListQuery { Year = context.Message.Year }, context.CancellationToken)
                    .ToListAsync();

                await context.RespondAsync<CalendarList>(new
                {
                    Calendars = result
                });

                _logger.LogInformation("The \"Get-Calendar-List\" request was processed...");
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "The calendar data was not retrieved");
                throw;
            }
        }
    }
}
