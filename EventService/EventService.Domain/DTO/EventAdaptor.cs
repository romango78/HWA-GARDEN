using HWA.GARDEN.EventService.DataAccess.Entities;
using HWA.GARDEN.EventService.Domain.Extensions;

namespace HWA.GARDEN.EventService.Domain.DTO
{
    internal class EventAdaptor : Event
    {
        public EventAdaptor(EventEntity eventEntity, int year)
        {
            // TODO: ADD PARAMETERS VALIDATION

            Id = eventEntity.Id;
            Group = new EventGroupAdapter(eventEntity.Group);
            Name = eventEntity.Name;
            Description = eventEntity.Description;
            StartDate = eventEntity.StartDt.ToDate(year);
            EndDate = eventEntity.EndDt.ToDate(year);
        }
    }
}
