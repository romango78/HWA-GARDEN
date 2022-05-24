using HWA.GARDEN.Contracts;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Domain
{
    public interface IEventService
    {
        IAsyncEnumerable<Event> GetEventsAsync(DateOnly startDate, DateOnly endDate, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
