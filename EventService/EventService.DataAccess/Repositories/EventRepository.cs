using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Entities;
using System.Data;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class EventRepository : BaseRepository, IEventRepository
    {
        public EventRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public IAsyncEnumerable<EventEntity> GetEventsAsync(int startDt, int endDt, int calendarId,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
