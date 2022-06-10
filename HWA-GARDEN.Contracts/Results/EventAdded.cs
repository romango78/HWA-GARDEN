using MassTransit;

namespace HWA.GARDEN.Contracts.Results
{
    public interface EventAdded : CorrelatedBy<Guid>
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public int GroupId { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
