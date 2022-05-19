using HWA.GARDEN.EventService.DataAccess.Repositories;

namespace HWA.GARDEN.EventService.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        ICalendarRepository CalendarRepository { get; }

        IEventRepository EventRepository { get; }
    }
}
