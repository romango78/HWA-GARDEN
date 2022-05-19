using HWA.GARDEN.EventService.DataAccess.Entities;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.DataAccess.Repositories
{
    public interface IEventRepository
    {
        IAsyncEnumerable<EventEntity> GetEventsAsync(int startDt, int endDt, int calendarId, 
            [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
