using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.EventService.Domain.Saga.States;
using MassTransit;

namespace HWA.GARDEN.EventService.Domain.Saga.StateMachines
{
    /// <summary>
    /// Represents the state of the create event routine.
    /// </summary>
    /// <remarks>
    /// The state diagram:
    ///     Initial --> CalendarCreationPending ┬-> GetOrAddCalendar.Completed --> EventGroupCreationPending ┬-> GetOrAddEventGroup.Completed --> EventCreationPending ┬-> Created --> Final
    ///                                         ├-> GetOrAddCalendar.Faulted --------┐                       ├-> GetOrAddEventGroup.Faulted                            ├-> CreateEvent.Faulted
    ///                                         └-> GetOrAddCalendar.TimeoutExpired -┤                       └-> GetOrAddEventGroup.TimeoutExpired                     └-> CreateEvent.TimeoutExpired
    ///                                                                                 └-> Faulted
    /// ... automata definitions:
    ///     C ::= {a1, g1, a2, g2, a3 }
    ///     Σ ⊆ {a!, a? | a ∈ C}
    ///     
    ///     A = [[A'┌►B' ꞏ A''┌►B'' ꞏ A3]]
    ///       = [
    ///         (
    ///             Ø,
    ///             Ø,
    ///             {s',s'',s5,s6,s7},
    ///             {s',s'',s5,s6,s7,q0,(●),(X)},
    ///             q0,
    ///             Inv,
    ///             {(q0,τ, true, Ø, s'),(s',[●],s''),(s',[X],(X)),(s'',[●],s5),(s'',[X],s7),  ,(s7,[●],[X])
    ///             (s2,[●],s3),(s2,[X],(X)),(s3,[●],(●)),(s3,[X],s4),(s4,[●],s5),(s5,[●],(X)))}
    ///         ),
    ///         {
    ///         (s',A'),(s'',A''),(s5,A3),(s6,B2),(s7,B1)}
    ///         ]
    ///     A' = [[A'1 || A'2┌►B1]]
    ///        = [
    ///          (
    ///             {addn1!, getn1!},
    ///             Ø,
    ///             {s1,s2},
    ///             {s1,s2,q0,q1,q2,(●),(X)},
    ///             q0,
    ///             I'nv,
    ///             {(q0,τ, true, Ø, s1),(s1,[●],q1),(s1,[X],s2),(q1,getn1!,true,Ø,(●)),(s2,[●],q2),(s2,[X],(X)),(q2,addn1!,true,Ø,(●))}
    ///          ),
    ///          {(s1,A'1),(s2,A'2)}
    ///          ]
    ///     A'' = [[A''1 || A''2┌►B2]]
    ///         = [
    ///           (
    ///             {addn2!, getn2!},
    ///             Ø,
    ///             {s3,s4},
    ///             {s3,s4,q0,q3,q4,(●),(X)},
    ///             q0,
    ///             I''nv,
    ///             {(q0,τ, true, Ø, s3),(s3,[●],q3),(s3,[X],s4),(q3,getn2!,true,Ø,(●)),(s4,[●],q4),(s4,[X],(X)),(q4,addn1!,true,Ø,(●))}
    ///           ),
    ///           {(s3,A''1),(s4,A''2)}
    ///           ]
    ///     B'' = [[B2]]
    ///         = [
    ///           (
    ///             {addn2!, getn2!},
    ///             Ø,
    ///             {s6}
    ///             {s6,q0,(●),(X)},
    ///             q0,
    ///             I''nv,
    ///             {(q0,addn1?, true, Ø, s6),(s6}
    ///     B'' = [[B2]]
    /// 
    /// States:
    ///     q0 ::= Initial
    ///     s1 ::= Tryint to get exist calendar by (Name, Year)
    ///     s2 ::= Adding a new calendar
    ///     s' ⊆ {s1,s2} ::= Getting or adding a calendar
    ///     s3 ::= Tryint to get exist event's group by (Name)
    ///     s4 ::= Adding a new event group
    ///     s'' ⊆ {s3,s4} ::= Getting or adding an event group
    ///     s5 ::= Creating a new event
    ///     s6 ::= Removing created event group
    ///     s7 ::= Removing created calendar
    /// </remarks>
    public sealed class CreateEventStateMachine : MassTransitStateMachine<CreateEventState>
    {
        public CreateEventStateMachine(IEndpointNameFormatter formatter)
        {
            InstanceState(m => m.CurrentState, CalendarAddingPending, EventGroupAddingPending, 
                EventAddingPending, Created, Faulted);

            Event(() => SagaCreateEvent);

            Request(() => GetOrAddCalendar,
                instance => instance.GetOrAddCalendarRequestId,
                cfg =>
                {
                    var endpoint = formatter.Consumer<IConsumer<GetOrAddCalendar>>();

                    cfg.ServiceAddress = new Uri($"queue:{endpoint}");
                    cfg.Timeout = TimeSpan.FromSeconds(60);
                });
            Request(() => GetOrAddEventGroup,
                instance => instance.GetOrAddEventGroupRequestId,
                cfg =>
                {
                    var endpoint = formatter.Consumer<IConsumer<GetOrAddEventGroup>>();

                    cfg.ServiceAddress = new Uri($"queue:{endpoint}");
                    cfg.Timeout = TimeSpan.FromSeconds(60);
                });
            Request(() => AddEvent,
                instance => instance.AddEventRequestId,
                cfg =>
                {
                    var endpoint = formatter.Consumer<IConsumer<AddEvent>>();

                    cfg.ServiceAddress = new Uri($"queue:{endpoint}");
                    cfg.Timeout = TimeSpan.FromSeconds(60);
                });

            Initially(
                When(SagaCreateEvent)
                    .TransitionTo(CalendarAddingPending)
                    .Then(ctx =>
                    {                        
                        ctx.Saga.CorrelationId = ctx.Message.CorrelationId;
                        ctx.Saga.Input = ctx.Message;
                        ctx.Saga.Output = new Contracts.Event();
                        ctx.Saga.ResponseAddress = ctx.ResponseAddress;
                        ctx.Saga.CreateEventRequestId = ctx.RequestId;
                    })
                    .Request(GetOrAddCalendar, ctx => 
                        ctx.Init<GetOrAddCalendar>(
                        new 
                        {
                            CorrelationId = ctx.Message.CorrelationId,
                            Name = ctx.Saga.Input?.CalendarName,
                            Description = ctx.Saga.Input?.CalendarDescription,
                            Year = ctx.Saga.Input?.CalendarYear
                        })));

            During(CalendarAddingPending,
                When(GetOrAddCalendar.Completed)
                    .TransitionTo(EventGroupAddingPending)
                    .Then(ctx =>
                    {
                        ctx.Saga.Output.Calendar = ctx.Message.Calendar;
                        ctx.Saga.WasCalendarCreated = !ctx.Message.IsAlreadyExists;
                    })
                    .Request(GetOrAddEventGroup, ctx =>
                        ctx.Init<GetOrAddEventGroup>(
                        new
                        {
                            CorrelationId = ctx.Message.CorrelationId,
                            Name = ctx.Saga.Input?.GroupName,
                            Description = ctx.Saga.Input?.GroupDescription
                        })),
                When(GetOrAddCalendar.Faulted)
                    .Then(ctx =>
                    {
                        var x = ctx.Saga;
                    })
                    .TransitionTo(Faulted),
                When(GetOrAddCalendar.TimeoutExpired)
                    .TransitionTo(Faulted),
                Ignore(SagaCreateEvent));

            During(EventGroupAddingPending,
                When(GetOrAddEventGroup.Completed)
                    .TransitionTo(EventAddingPending)
                    .Then(ctx =>
                    {
                        ctx.Saga.Output.Group = ctx.Message.EventGroup;
                        ctx.Saga.WasEventGroupCreated = !ctx.Message.IsAlreadyExists;
                    })
                    .Request(AddEvent, ctx =>
                        ctx.Init<AddEvent>(
                        new
                        {
                            CorrelationId = ctx.Message.CorrelationId,
                            CalendarId = ctx.Saga.Output?.Calendar?.Id,
                            GroupId = ctx.Saga.Output?.Group?.Id,
                            Name = ctx.Saga.Input?.Name,
                            Description = ctx.Saga.Input?.Description,
                            StartDate = ctx.Saga.Input?.StartDate,
                            EndDate = ctx.Saga.Input?.EndDate
                        })),
                When(GetOrAddEventGroup.Faulted)
                    .TransitionTo(Faulted),
                When(GetOrAddEventGroup.TimeoutExpired)
                    .TransitionTo(Faulted),
                Ignore(SagaCreateEvent));

            During(EventAddingPending,
                When(AddEvent.Completed)
                    .TransitionTo(Created)
                    .ThenAsync(async ctx =>
                    {
                        ctx.Saga.Output.Id = ctx.Message.Id;
                        ctx.Saga.Output.Name = ctx.Message.Name;
                        ctx.Saga.Output.Description = ctx.Message.Description;
                        ctx.Saga.Output.StartDate = ctx.Message.StartDate;
                        ctx.Saga.Output.EndDate = ctx.Message.EndDate;
                        if (ctx.Saga.ResponseAddress != null)
                        {
                            ISendEndpoint? endpoint = await ctx.GetSendEndpoint(ctx.Saga.ResponseAddress);
                            await endpoint.Send<EventCreated>(new 
                                { 
                                    CorrelationId = ctx.Message.CorrelationId,
                                    Event = ctx.Saga.Output 
                                }, 
                                sendCtx => 
                                {
                                    sendCtx.RequestId = ctx.Saga.CreateEventRequestId;
                                });
                        }
                    }),
                When(AddEvent.Faulted)
                    .TransitionTo(Faulted),
                When(AddEvent.TimeoutExpired)
                    .TransitionTo(Faulted),
                Ignore(SagaCreateEvent));

            /*DuringAny(
                When(Created)
                    .);*/


            //SetCompletedWhenFinalized();
        }

        public State CalendarAddingPending { get; }

        public State EventGroupAddingPending { get; }

        public State EventAddingPending { get; }

        public State Created { get; }

        public State Faulted { get; }

        public Event<CreateEvent> SagaCreateEvent { get; }

        public Request<CreateEventState, GetOrAddCalendar, CalendarAdded> GetOrAddCalendar { get; }

        public Request<CreateEventState, GetOrAddEventGroup, EventGroupAdded> GetOrAddEventGroup { get; }

        public Request<CreateEventState, AddEvent, EventAdded> AddEvent { get; }
    }
}
