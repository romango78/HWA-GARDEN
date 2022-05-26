using HWA.GARDEN.CalendarService.Data.Entities;

namespace HWA.GARDEN.CalendarService.Data.Repositories
{
    public interface ICalendarRepository
    {
        IAsyncEnumerable<CalendarEntity> GetListAsync(int year, CancellationToken cancellationToken = default);
    }
}
