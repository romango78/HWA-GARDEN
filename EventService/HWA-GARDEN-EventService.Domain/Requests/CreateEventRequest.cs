using HWA.GARDEN.Contracts;
using MediatR;

namespace HWA.GARDEN.EventService.Domain.Requests
{
    public sealed class CreateEventRequest : IRequest<Event>
    {
        public string CalendarName { get; set; }

        public string? CalendarDescription { get; set; }

        public int CalendarYear { get; set; }

        public string GroupName { get; set; }

        public string? GroupDescription { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

    }
}
