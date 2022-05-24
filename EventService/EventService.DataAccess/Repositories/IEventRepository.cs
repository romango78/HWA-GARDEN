using HWA.GARDEN.EventService.Data.Entities;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public interface IEventRepository
    {
        IAsyncEnumerable<EventEntity> GetEventsAsync(int startDt, int endDt, int calendarId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
