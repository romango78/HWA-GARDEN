using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.Utilities.Validation;

namespace HWA.GARDEN.EventService.Domain.Adaptors
{
    public class CalendarAdaptor : Calendar
    {
        public CalendarAdaptor(CalendarEntity calendarEntity)
        {
            Requires.NotNull(calendarEntity, nameof(calendarEntity));

            Name = calendarEntity.Name;
            Year = calendarEntity.Year;
        }
    }
}
