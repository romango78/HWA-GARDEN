using HWA.GARDEN.CalendarService.Data.Repositories;

namespace HWA.GARDEN.CalendarService.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICalendarRepository CalendarRepository { get; }

        public void Commit();
    }
}
