using MassTransit;

namespace HWA.GARDEN.Contracts.Results
{
    public interface CalendarAdded : CorrelatedBy<Guid>
    {
        public Calendar Calendar { get;}

        public bool IsAlreadyExists { get; }
    }
}
