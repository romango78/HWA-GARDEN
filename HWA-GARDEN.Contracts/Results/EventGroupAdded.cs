using MassTransit;

namespace HWA.GARDEN.Contracts.Results
{
    public interface EventGroupAdded : CorrelatedBy<Guid>
    {
        public EventGroup EventGroup { get; }

        public bool IsAlreadyExists { get; }
    }
}
