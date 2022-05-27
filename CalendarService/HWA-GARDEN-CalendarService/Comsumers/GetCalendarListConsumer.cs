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

        public GetCalendarListConsumer(IMediator mediator)
        {
            Requires.NotNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        async Task IConsumer<GetCalendarList>.Consume(ConsumeContext<GetCalendarList> context)
        {
            IList<Calendar> result = 
                await _mediator.CreateStream(new CalendarListQuery { Year = context.Message.Year }, context.CancellationToken)                
                .ToListAsync();

            await context.RespondAsync<CalendarList>(new
            {
                Calendars = result
            });
        }
    }
}
