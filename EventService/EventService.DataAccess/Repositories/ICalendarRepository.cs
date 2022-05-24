using HWA.GARDEN.EventService.Data.Entities;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public interface ICalendarRepository
    {
        Task<CalendarEntity> GetCalendarAsync(int year, CancellationToken cancellationToken);
    }
}
