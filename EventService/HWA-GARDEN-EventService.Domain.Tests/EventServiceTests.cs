using FluentAssertions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.EventService.Data.Repositories;
using Moq;

namespace HWA.GARDEN.EventService.Domain.Tests
{
    [Trait("TestCategory", "UnitTest")]
    public class EventServiceTests
    {
        [Fact]
        public async Task Should_GetEventsForTimePeriod_InsideOneYear()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 2, 1);
            DateOnly end = new DateOnly(2022, 3, 1);

            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity()));
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.Is<int>(a => a == 2022), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity { Id = 1, Name = "2022", Year = 2022 }));

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.Is<int>(a => a == 32), It.Is<int>(a => a == 60)
                    , It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 1, EndDt = 33, Name = "Event 1", GroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 60, Name = "Event 2", GroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 40, Name = "Event 3", GroupId = 1 },
                    new EventEntity { StartDt = 1, EndDt = 60, Name = "Event 4" , GroupId = 1 }
                }).ToAsyncEnumerable());

            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new EventGroupEntity { Id = 1, Name = "G1" }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var sut = new EventService(unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.GetEventsAsync(start, end, CancellationToken.None))
            {
                item.Should().Match<Event>(m => m.StartDate <= end && m.EndDate >= start
                    && m.Group.Name.Equals("G1", StringComparison.OrdinalIgnoreCase)
                    && m.Calendar.Year == 2022);
                count++;
            }
            count.Should().Be(4);
        }

        [Fact]
        public async Task Should_GetEventsForTimePeriod_InsideTwoYears()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 10, 1);
            DateOnly end = new DateOnly(2023, 3, 1);

            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity()));
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.Is<int>(a => a == 2022), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity { Id = 1, Name = "2022", Year = 2022 }));
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.Is<int>(a => a == 2023), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity { Id = 2, Name = "2023", Year = 2023 }));

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.Is<int>(a => a == 274), It.Is<int>(a => a == 365)
                    , It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 200, EndDt = 365, Name = "Event 5", GroupId = 1 },
                    new EventEntity { StartDt = 283, EndDt = 360, Name = "Event 6", GroupId = 1 }
                }).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 60)
                    , It.Is<int>(a => a == 2), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 1, EndDt = 33, Name = "Event 1", GroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 60, Name = "Event 2", GroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 40, Name = "Event 3", GroupId = 1 },
                    new EventEntity { StartDt = 1, EndDt = 60, Name = "Event 4", GroupId = 1 }
                }).ToAsyncEnumerable());


            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new EventGroupEntity { Id = 1, Name = "G1" }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var sut = new EventService(unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.GetEventsAsync(start, end, CancellationToken.None))
            {
                item.Should().Match<Event>(m => m.StartDate <= end && m.EndDate >= start
                    && m.Group.Name.Equals("G1", StringComparison.OrdinalIgnoreCase)
                    && (m.Calendar.Year == 2022 || m.Calendar.Year == 2023));
                count++;
            }
            count.Should().Be(6);
        }

        [Fact]
        public async Task Should_ThrowException_WhenEndDateLessStartDate()
        {
            // Arrange
            DateOnly start = new DateOnly(2023, 10, 1);
            DateOnly end = new DateOnly(2022, 3, 1);

            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity()));

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                return uow;
            };

            var sut = new EventService(unitOfWorkFactory);

            // Act 
            Func<Task> func = async () => await sut.GetEventsAsync(start, end, CancellationToken.None).FirstAsync();

            // Asserts
            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Should_OperationCancelled_ByRequest()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 2, 1);
            DateOnly end = new DateOnly(2022, 3, 1);

            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetCalendarAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(new CalendarEntity()));

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetEventsAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                return uow;
            };

            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;

            var sut = new EventService(unitOfWorkFactory);

            // Act 
            Func<Task> func = async () => await sut.GetEventsAsync(start, end, cancellationToken)
                .GetAsyncEnumerator(cancellationToken)
                .MoveNextAsync();

            source.Cancel();

            // Asserts
            await func.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}