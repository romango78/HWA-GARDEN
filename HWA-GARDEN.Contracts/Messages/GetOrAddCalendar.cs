using MassTransit;

namespace HWA.GARDEN.Contracts.Messages
{
    public interface GetOrAddCalendar : CorrelatedBy<Guid>
    {
        public string Name { get; }

        public string Description { get; }

        public int Year { get; }
    }
}
