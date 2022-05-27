using HWA.GARDEN.Contracts;

namespace HWA.GARDEN.EventService.Domain
{
    public interface IEventService
    {
        IAsyncEnumerable<Event> GetEventsAsync(DateOnly startDate, DateOnly endDate, 
            CancellationToken cancellationToken = default);
    }
}
