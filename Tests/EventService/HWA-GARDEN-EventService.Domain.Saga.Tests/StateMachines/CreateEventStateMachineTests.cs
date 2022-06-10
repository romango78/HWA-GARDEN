using FluentAssertions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.EventService.Domain.Saga.StateMachines;
using HWA.GARDEN.EventService.Domain.Saga.States;
using HWA.GARDEN.Tests.Utilities;
using MassTransit;
using MassTransit.Testing;
using MassTransit.Transactions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit.Abstractions;

namespace HWA.GARDEN.EventService.Domain.Saga.Tests.StateMachines
{
    public class CreateEventStateMachineTests : StateMachineTestFixture<CreateEventStateMachine, CreateEventState>
    {
        public CreateEventStateMachineTests(ITestOutputHelper outputHelper)
            :base(outputHelper)
        {            
        }

        protected override void ConfigureServices(IServiceCollection collection)
        {
            collection.AddSingleton<ITestContextContainer, TestContextContainer>();
        }

        protected override void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
        {
            configurator.AddConsumer<ConsumerMock<GetOrAddCalendar>>();
            configurator.AddConsumer<ConsumerMock<GetOrAddEventGroup>>();
            configurator.AddConsumer<ConsumerMock<AddEvent>>();            
        }

        [Fact]
        public async Task ShouldSuccessfullyProcess_EventCreationFlow_WhenNoIssuesOccurred()
        {
            // Arrange
            const int CalendarId = 323;
            const int EventId = 9012;
            const int EventGroupId = 8919;

            var testCtxContainer = Provider.GetRequiredService<ITestContextContainer>();

            var getOrCreateCalendarConsumer = Mock.Of<IConsumer<GetOrAddCalendar>>();
            Mock.Get(getOrCreateCalendarConsumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<GetOrAddCalendar>>()))
                .Callback<ConsumeContext<GetOrAddCalendar>>(async (ctx) =>
                {
                    await ctx.RespondAsync<CalendarAdded>(new
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        Calendar = new Calendar
                        {
                            Id = CalendarId,
                            Name = ctx.Message.Name,
                            Description = ctx.Message.Description,
                            Year = ctx.Message.Year
                        },
                        IsAlreadyExists = false
                    });
                })
                .Returns(Task.FromResult(0));
            testCtxContainer.AddMockedConsumer(getOrCreateCalendarConsumer);

            var getOrCreateEventGroupConsumer = Mock.Of<IConsumer<GetOrAddEventGroup>>();
            Mock.Get(getOrCreateEventGroupConsumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<GetOrAddEventGroup>>()))
                .Callback<ConsumeContext<GetOrAddEventGroup>>(async (ctx) =>
                {
                    await ctx.RespondAsync<EventGroupAdded>(new
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        EventGroup = new EventGroup
                        {
                            Id = EventGroupId,
                            Name = ctx.Message.Name,
                            Description = ctx.Message.Description
                        },
                        IsAlreadyExists = false
                    });
                })
                .Returns(Task.FromResult(0));
            testCtxContainer.AddMockedConsumer(getOrCreateEventGroupConsumer);

            var createEventConsumer = Mock.Of<IConsumer<AddEvent>>();
            Mock.Get(createEventConsumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<AddEvent>>()))
                .Callback<ConsumeContext<AddEvent>>(async (ctx) =>
                {
                    await ctx.RespondAsync<EventAdded>(new
                    {
                        CorrelationId = ctx.Message.CorrelationId,
                        Id = EventId,
                        CalendarId = ctx.Message.CalendarId,
                        GroupId = ctx.Message.GroupId,
                        Name = ctx.Message.Name,
                        Description = ctx.Message.Description,
                        StartDate = ctx.Message.StartDate,
                        EndDate = ctx.Message.EndDate
                    });
                })
                .Returns(Task.FromResult(0));
            testCtxContainer.AddMockedConsumer(createEventConsumer);

            await TestHarness.Start();

            var sagaId = Guid.NewGuid();

            var eventData = new
            {
                CorrelationId = sagaId,
                CalendarName = "Calendar 2022",
                CalendarDescription = "Calendar",
                CalendarYear = 2022,
                GroupName = "Event Group 1",
                GroupDescription = "Group",
                Name = "Event 1",
                Description = "Event",
                StartDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 2, 1)
            };

            // Act
            IRequestClient<CreateEvent>? requestClient = TestHarness.GetRequestClient<CreateEvent>();
            Response<EventCreated>? result = await requestClient.GetResponse<EventCreated>(eventData, CancellationToken.None);

            // Asserts
            (await TestHarness.Published.Any<CreateEvent>()).Should().BeTrue();

            ISagaStateMachineTestHarness<CreateEventStateMachine, CreateEventState>? sagaHarness =
                TestHarness.GetSagaStateMachineHarness<CreateEventStateMachine, CreateEventState>();

            (await sagaHarness.Exists(sagaId, s => s.Created)).HasValue.Should().BeTrue();

            (await sagaHarness.Consumed.Any<CreateEvent>()).Should().BeTrue();
            (await sagaHarness.Consumed.Any<CalendarAdded>()).Should().BeTrue();
            (await sagaHarness.Consumed.Any<EventGroupAdded>()).Should().BeTrue();
            (await sagaHarness.Consumed.Any<EventAdded>()).Should().BeTrue();

            result.Should().NotBeNull();
            result.Message.Should().NotBeNull()
                .And.Match<EventCreated>(c => c.CorrelationId == sagaId && c.Event.Id == EventId &&
                    c.Event.Name.Equals(eventData.Name, StringComparison.OrdinalIgnoreCase) &&
                    c.Event.Description.Equals(eventData.Description, StringComparison.OrdinalIgnoreCase) &&
                    c.Event.StartDate.Equals(eventData.StartDate) &&
                    c.Event.EndDate.Equals(eventData.EndDate));
            result.Message.Event.Calendar.Should().NotBeNull()
                .And.Match<Calendar>(c => c.Id == CalendarId &&
                    c.Name.Equals(eventData.CalendarName, StringComparison.OrdinalIgnoreCase) &&
                    c.Description.Equals(eventData.CalendarDescription, StringComparison.OrdinalIgnoreCase) &&
                    c.Year == eventData.CalendarYear);
            result.Message.Event.Group.Should().NotBeNull()
                .And.Match<EventGroup>(c => c.Id == EventGroupId &&
                    c.Name.Equals(eventData.GroupName, StringComparison.OrdinalIgnoreCase) &&
                    c.Description.Equals(eventData.GroupDescription, StringComparison.OrdinalIgnoreCase));

        }

        [Fact]
        public async Task Test()
        {
            // Arrange
            var testCtxContainer = Provider.GetRequiredService<ITestContextContainer>();

            var getOrCreateCalendarConsumer = Mock.Of<IConsumer<GetOrAddCalendar>>();
            Mock.Get(getOrCreateCalendarConsumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<GetOrAddCalendar>>()))
                .ThrowsAsync(new Exception("Something went wrong."));
            testCtxContainer.AddMockedConsumer(getOrCreateCalendarConsumer);

            await TestHarness.Start();

            var sagaId = Guid.NewGuid();

            // Act
            await TestHarness.Bus.Publish<CreateEvent>(new
            {
                CorrelationId = sagaId,
                CalendarName = "Calendar 2022",
                CalendarDescription = "Calendar",
                CalendarYear = 2022,
                GroupName = "Event Group 1",
                GroupDescription = "Group",
                Name = "Event 1",
                Description = "Event",
                StartDate = new DateTime(2022, 1, 1),
                EndDate = new DateTime(2022, 2, 1)
            }, CancellationToken.None);

            // Asserts
            (await TestHarness.Published.Any<CreateEvent>()).Should().BeTrue();

            ISagaStateMachineTestHarness<CreateEventStateMachine, CreateEventState>? sagaHarness =
                TestHarness.GetSagaStateMachineHarness<CreateEventStateMachine, CreateEventState>();

            (await sagaHarness.Exists(sagaId, s => s.CalendarAddingPending)).HasValue.Should().BeTrue();

            await Task.Delay(1000);
            (await sagaHarness.Exists(sagaId, s => s.Faulted)).HasValue.Should().BeTrue();

            (await sagaHarness.Consumed.Any<CreateEvent>()).Should().BeTrue();
            (await sagaHarness.Consumed.Any<CalendarAdded>()).Should().BeFalse();
            (await sagaHarness.Consumed.Any<EventGroupAdded>()).Should().BeFalse();
            (await sagaHarness.Consumed.Any<EventAdded>()).Should().BeFalse();
        }
    }
}