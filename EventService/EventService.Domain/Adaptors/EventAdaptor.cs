using HWA.GARDEN.EventService.Domain.Extensions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using HWA.GARDEN.EventService.Data.Entities;

namespace HWA.GARDEN.EventService.Domain.Adaptors
{
    internal class EventAdaptor : Event
    {
        public EventAdaptor(EventEntity eventEntity, EventGroupEntity eventGroupEntity, CalendarEntity calendarEntity)
        {
            Requires.NotNull(eventEntity, nameof(eventEntity));
            Requires.NotNull(calendarEntity, nameof(calendarEntity));

            Id = eventEntity.Id;
            Calendar = new CalendarAdaptor(calendarEntity);
            Group = eventGroupEntity != null ? new EventGroupAdapter(eventGroupEntity) : null;
            Name = eventEntity.Name;
            Description = eventEntity.Description;
            StartDate = eventEntity.StartDt.ToDate(Calendar.Year);
            EndDate = eventEntity.EndDt.ToDate(Calendar.Year);
        }
    }
}
