using HWA.GARDEN.Utilities.Extensions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;
using HWA.GARDEN.EventService.Data.Entities;

namespace HWA.GARDEN.EventService.Domain.Adaptors
{
    internal class EventAdaptor : Event
    {
        public EventAdaptor(EventEntity eventEntity, EventGroupEntity eventGroupEntity, Calendar calendar)
        {
            Requires.NotNull(eventEntity, nameof(eventEntity));
            Requires.NotNull(calendar, nameof(calendar));

            Id = eventEntity.Id;
            Calendar = calendar;
            Group = eventGroupEntity != null ? new EventGroupAdapter(eventGroupEntity) : null;
            Name = eventEntity.Name;
            Description = eventEntity.Description;
            StartDate = eventEntity.StartDt.ToDate(Calendar.Year);
            EndDate = eventEntity.EndDt.ToDate(Calendar.Year);
        }
    }
}
