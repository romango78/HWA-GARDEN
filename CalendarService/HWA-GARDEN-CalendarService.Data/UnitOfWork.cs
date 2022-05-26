using HWA.GARDEN.CalendarService.Data.Repositories;
using System.Data.Common;
using HWA.GARDEN.Utilities.Validation;
using HWA.GARDEN.Data;

namespace HWA.GARDEN.CalendarService.Data
{
    public class UnitOfWork : BaseUnitOfWork, IUnitOfWork
    {
        public UnitOfWork(IConnectionFactory connectionFactory, Func<DbTransaction, ICalendarRepository> calendarRepoFactory)
            :base(connectionFactory)
        {
            Requires.NotNull(connectionFactory, nameof(connectionFactory));
            Requires.NotNull(calendarRepoFactory, nameof(calendarRepoFactory));

            CalendarRepository = calendarRepoFactory(Transaction);
        }

        public ICalendarRepository CalendarRepository { get; }
    }
}
