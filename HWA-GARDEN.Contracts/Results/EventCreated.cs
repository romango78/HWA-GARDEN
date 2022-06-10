using MassTransit;

namespace HWA.GARDEN.Contracts.Results
{
    public interface EventCreated : CorrelatedBy<Guid>
    {
        public Event Event { get; }
    }
}
