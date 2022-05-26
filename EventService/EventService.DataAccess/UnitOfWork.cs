using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Repositories;
using System.Data.Common;

namespace HWA.GARDEN.EventService.Data
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(IConnectionFactory connectionFactory, Func<DbTransaction, ICalendarRepository> calendarRepoFactory,
            Func<DbTransaction, IEventRepository> eventRepoFactory, Func<DbTransaction, IEventGroupRepository> eventGroupRepoFactory)
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
