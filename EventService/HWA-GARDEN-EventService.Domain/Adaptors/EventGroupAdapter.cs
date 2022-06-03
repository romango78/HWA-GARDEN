using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using HWA.GARDEN.EventService.Data.Entities;

namespace HWA.GARDEN.EventService.Domain.Adaptors
{
    public class EventGroupAdapter : EventGroup
    {
        public EventGroupAdapter(EventGroupEntity eventGroupEntity)
        {
            Requires.NotNull(eventGroupEntity, nameof(eventGroupEntity));

            Id = eventGroupEntity.Id;
            Name = eventGroupEntity.Name;
            Description = eventGroupEntity.Description;
        }
    }
}
