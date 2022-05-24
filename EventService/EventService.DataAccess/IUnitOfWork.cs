using HWA.GARDEN.EventService.Data.Repositories;

namespace HWA.GARDEN.EventService.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICalendarRepository CalendarRepository { get; }

        IEventRepository EventRepository { get; }

        IEventGroupRepository EventGroupRepository { get; }

        public void Commit();
    }
}
