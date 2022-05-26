using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Domain.Requests;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    internal sealed class CalendarListQueryHandler
        : IRequestHandler<CalendarListQuery, Calendar>
    {
        public CalendarListQueryHandler()
        {

        }

        public Task<Calendar> Handle(CalendarListQuery request, CancellationToken cancellationToken)
        {
            /*
            CalendarEntity calendar = await uow.CalendarRepository
                .GetAsync(startDate.Year, cancellationToken)
                .ConfigureAwait(false) ?? new CalendarEntity { Year = startDate.Year };*/

            return Task.FromResult(new Calendar { Year = request.Year });
        }
    }
}
