using HWA.GARDEN.EventService.Data.Entities;

namespace HWA.GARDEN.EventService.Data.Repositories
{
    public interface IEventGroupRepository
    {
        Task<EventGroupEntity> GetAsync(int id, CancellationToken cancellationToken);
    }
}
