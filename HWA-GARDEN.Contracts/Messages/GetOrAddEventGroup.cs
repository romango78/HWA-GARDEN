using MassTransit;

namespace HWA.GARDEN.Contracts.Messages
{
    public interface GetOrAddEventGroup : CorrelatedBy<Guid>
    {
        public string Name { get; }

        public string? Description { get; }
    }
}
