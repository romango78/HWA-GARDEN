using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Repositories;
using System.Data;

namespace HWA.GARDEN.EventService.Data
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(IConnectionFactory connectionFactory, Func<IDbTransaction, ICalendarRepository> calendarRepoFactory,
            Func<IDbTransaction, IEventRepository> eventRepoFactory, Func<IDbTransaction, IEventGroupRepository> eventGroupRepoFactory)
            :base(connectionFactory)
        {
            CalendarRepository = calendarRepoFactory(Transaction);
            EventRepository = eventRepoFactory(Transaction);
            EventGroupRepository = eventGroupRepoFactory(Transaction);
        }

        public ICalendarRepository CalendarRepository { get; }

        public IEventRepository EventRepository { get; }

        public IEventGroupRepository EventGroupRepository { get; }

    }
}
