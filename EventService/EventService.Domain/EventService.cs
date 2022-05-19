using HWA.GARDEN.EventService.DataAccess;
using HWA.GARDEN.EventService.DataAccess.Entities;
using HWA.GARDEN.EventService.Domain.DTO;
using HWA.GARDEN.EventService.Domain.Extensions;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain
{
    public class EventService : IEventService
    {
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public EventService(Func<IUnitOfWork> unitOfWorkFactory)
        {
            // TODO: ADD PARAMETERS VALIDATION

            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async IAsyncEnumerable<Event> GetEventsAsync(DateOnly startDate, DateOnly endDate,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (startDate > endDate)
            {
                throw new InvalidOperationException("The satrt date should be equal or less than end date.");
            }

            do
            {
                await foreach (Event item in 
                    GetEventsForPeriodAsync(startDate, endDate)
                    .WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return item;
                }
                
                startDate = startDate.Year != endDate.Year 
                    ? GetLastYearDay(startDate.Year).AddDays(1) 
                    : endDate;
            } while (startDate != endDate);
        }

        private async IAsyncEnumerable<Event> GetEventsForPeriodAsync(DateOnly startDate, DateOnly endDate, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if(startDate.Year != endDate.Year)
            {
                endDate = GetLastYearDay(startDate.Year);
            }
            
            using(IUnitOfWork uow = _unitOfWorkFactory())
            {
                CalendarEntity calendar = await uow.CalendarRepository.GetCalendarAsync(startDate.Year, cancellationToken);

                await foreach (EventEntity item in
                    uow.EventRepository.GetEventsAsync(startDate.ToDayOfYear(), endDate.ToDayOfYear(), calendar.Id)
                    .WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    yield return new EventAdaptor(item, startDate.Year);
                }
            }
        }

        private DateOnly GetLastYearDay(int year)
        {
            return new DateOnly(year, 12, 31);
        }
    }
}
