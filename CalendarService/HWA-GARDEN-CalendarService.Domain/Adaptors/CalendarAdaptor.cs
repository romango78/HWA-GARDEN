using HWA.GARDEN.CalendarService.Data.Entities;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Utilities.Validation;

namespace HWA.GARDEN.CalendarService.Domain.Adaptors
{
    public class CalendarAdaptor : Calendar
    {
        public CalendarAdaptor(CalendarEntity calendarEntity)
        {
            Requires.NotNull(calendarEntity, nameof(calendarEntity));

            Id = calendarEntity.Id;
            Name = calendarEntity.Name;
            Description = calendarEntity.Description;
            Year = calendarEntity.Year;
        }
    }
}
