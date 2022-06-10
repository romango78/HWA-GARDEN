using MassTransit;

namespace HWA.GARDEN.Contracts.Messages
{
    public interface AddEvent : CorrelatedBy<Guid>
    {
        public int CalendarId { get; }

        public int GroupId { get; }

        public string Name { get; }

        public string? Description { get; }

        public DateTime StartDate { get; }

        public DateTime EndDate { get; }
    }
}
