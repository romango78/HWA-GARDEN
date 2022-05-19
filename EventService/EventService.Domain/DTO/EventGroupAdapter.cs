using HWA.GARDEN.EventService.DataAccess.Entities;

namespace HWA.GARDEN.EventService.Domain.DTO
{
    public class EventGroupAdapter : EventGroup
    {
        public EventGroupAdapter(EventGroupEntity eventGroupEntity)
        {
            // TODO: ADD PARAMETERS VALIDATION

            Id = eventGroupEntity.Id;
            Name = eventGroupEntity.Name;
            Description = eventGroupEntity.Description;
        }
    }
}
