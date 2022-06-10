using MassTransit;

namespace HWA.GARDEN.Contracts.Messages
{
    public interface CreateEvent : CorrelatedBy<Guid>
    {
        public string CalendarName { get; }

        public string? CalendarDescription { get; }

        public int CalendarYear { get; }

        public string GroupName { get; }

        public string? GroupDescription { get; }

        public string Name { get; }

        public string? Description { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
    }
}
