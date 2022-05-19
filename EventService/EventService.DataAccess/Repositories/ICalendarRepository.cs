using HWA.GARDEN.EventService.DataAccess.Entities;

namespace HWA.GARDEN.EventService.DataAccess.Repositories
{
    public interface ICalendarRepository
    {
        Task<CalendarEntity> GetCalendarAsync(int year, CancellationToken cancellationToken);
    }
}
