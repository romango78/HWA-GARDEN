using HWA.GARDEN.Contracts.Messages;
using MassTransit;

namespace HWA.GARDEN.EventService.Domain.Saga.States
{
    public sealed class CreateEventState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }

        public int CurrentState { get; set; }

        public Uri? ResponseAddress { get; set; }

        public Guid? AddEventRequestId { get; set; }

        public Guid? CreateEventRequestId { get; set; }

        public Guid? GetOrAddCalendarRequestId { get; set; }

        public Guid? GetOrAddEventGroupRequestId { get; set; }

        public CreateEvent? Input { get; set; }

        public Contracts.Event? Output { get; set; }

        public bool WasCalendarCreated { get; set; }

        public bool WasEventGroupCreated { get; set; }
    }
}
