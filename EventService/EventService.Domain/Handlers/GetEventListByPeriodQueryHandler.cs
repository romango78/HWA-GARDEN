using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.EventService.Domain.Adaptors;
using HWA.GARDEN.EventService.Domain.Requests;
using HWA.GARDEN.Utilities.Extensions;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain.Handlers
{
    public sealed class GetEventListByPeriodQueryHandler
        : IStreamRequestHandler<GetEventListByPeriodQuery, Event>
    {
        private readonly IMediator _mediator;
        private readonly Func<IUnitOfWork> _unitOfWorkFactory;

        public GetEventListByPeriodQueryHandler(IMediator mediator, Func<IUnitOfWork> unitOfWorkFactory)
        {
            Requires.NotNull(mediator, nameof(mediator));
            Requires.NotNull(unitOfWorkFactory, nameof(unitOfWorkFactory));

            _mediator = mediator;
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public async IAsyncEnumerable<Event> Handle(GetEventListByPeriodQuery request
            , [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (request.StartDate > request.EndDate)
            {
                throw new InvalidOperationException("The satrt date should be equal or less than end date.");
            }

            do
            {
                await foreach (Event item in
                    GetEventsForPeriodAsync(request.StartDate, request.EndDate)
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false))
                {
                    yield return item;
                }

                request.StartDate = request.StartDate.Year != request.EndDate.Year
                    ? GetLastYearDay(request.StartDate.Year).AddDays(1)
                    : request.EndDate;
            } while (request.StartDate != request.EndDate);
        }

        private async IAsyncEnumerable<Event> GetEventsForPeriodAsync(DateOnly startDate, DateOnly endDate,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (Calendar? calendar in
                _mediator.CreateStream(new GetCalendarListQuery { Year = startDate.Year }, cancellationToken)
                .ConfigureAwait(false))
            {
                await foreach (Event? item in GetEventsForPeriodInSpecificCalendarAsync(startDate, endDate, calendar)
                    .WithCancellation(cancellationToken)
                    .ConfigureAwait(false))
                {
                    yield return item;
                }
            }
        }

        private async IAsyncEnumerable<Event> GetEventsForPeriodInSpecificCalendarAsync(DateOnly startDate, DateOnly endDate,
            Calendar calendar, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (startDate.Year != endDate.Year)
            {
                endDate = GetLastYearDay(startDate.Year);
            }

            using (IUnitOfWork uow = _unitOfWorkFactory())
            {

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
