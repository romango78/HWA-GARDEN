using HWA.GARDEN.Common.Data;
using HWA.GARDEN.EventService.Data.Entities;
using System.Data;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public class EventGroupRepository : BaseRepository, IEventGroupRepository
    {
        public EventGroupRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public Task<EventGroupEntity> GetAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
