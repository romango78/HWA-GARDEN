using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.EventService.Domain.Adaptors;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Extensions;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain
{
    public class EventService : IEventService
    {
        private readonly IMediator _mediator;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public EventService(IMediator mediator, Func<IUnitOfWork> unitOfWorkFactory)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            _mediator = mediator;
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

                Calendar calendar = 
                    await _mediator.Send(new CalendarListQuery { Year = startDate.Year }, cancellationToken);

                IEnumerable<EventGroupEntity> eventGroupList =
                    await uow.EventGroupRepository
                        .GetAsync(startDate.ToDayOfYear(), endDate.ToDayOfYear(), calendar.Id, cancellationToken)
                        .ConfigureAwait(false);

                await foreach (EventEntity item in
                    uow.EventRepository.GetAsync(startDate.ToDayOfYear(), endDate.ToDayOfYear(), calendar.Id)
                    .WithCancellation(cancellationToken).ConfigureAwait(false))
                {
                    EventGroupEntity eventGroup = eventGroupList.FirstOrDefault(w => w.Id == item.EventGroupId);
                    yield return new EventAdaptor(item, eventGroup, calendar);
                }
            }
        }

        private DateOnly GetLastYearDay(int year)
        {
            return new DateOnly(year, 12, 31);
        }
    }
}
