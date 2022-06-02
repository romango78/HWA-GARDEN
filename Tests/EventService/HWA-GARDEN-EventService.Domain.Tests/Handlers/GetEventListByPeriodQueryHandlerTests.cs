using FluentAssertions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.EventService.Data;
using HWA.GARDEN.EventService.Data.Entities;
using HWA.GARDEN.EventService.Data.Repositories;
using HWA.GARDEN.EventService.Domain.Handlers;
using HWA.GARDEN.EventService.Domain.Requests;
using MediatR;
using Moq;

namespace HWA.GARDEN.EventService.Domain.Tests.Handlers
{
    [Trait("TestCategory", "UnitTest")]
    public class GetEventListByPeriodQueryHandlerTests
    {
        [Fact]
        public async Task Should_GetEventsForTimePeriod_InsideOneYear()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 2, 1);
            DateOnly end = new DateOnly(2022, 3, 1);

            var mediator = Mock.Of<IMediator>();
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<GetCalendarListQuery>(a => a.Year == start.Year), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 1, Name = "2022", Year = 2022 }
                }).ToAsyncEnumerable());

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 32), It.Is<int>(a => a == 60)
                    , It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 1, EndDt = 33, Name = "Event 1", EventGroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 60, Name = "Event 2", EventGroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 40, Name = "Event 3", EventGroupId = 1 },
                    new EventEntity { StartDt = 1, EndDt = 60, Name = "Event 4" , EventGroupId = 1 }
                }).ToAsyncEnumerable());

            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IEnumerable<EventGroupEntity>)new[] { new EventGroupEntity { Id = 1, Name = "G1" } }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var sut = new GetEventListByPeriodQueryHandler(mediator, unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }
                , CancellationToken.None))
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

            var mediator = Mock.Of<IMediator>();
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<GetCalendarListQuery>(a => a.Year == start.Year), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 1, Name = "2022", Year = 2022 }
                }).ToAsyncEnumerable());
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<GetCalendarListQuery>(a => a.Year == end.Year), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 2, Name = "2023", Year = 2023 }
                }).ToAsyncEnumerable());

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 274), It.Is<int>(a => a == 365)
                    , It.Is<int>(a => a == 1), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 200, EndDt = 365, Name = "Event 5", EventGroupId = 1 },
                    new EventEntity { StartDt = 283, EndDt = 360, Name = "Event 6", EventGroupId = 1 }
                }).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 60)
                    , It.Is<int>(a => a == 2), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new EventEntity { StartDt = 1, EndDt = 33, Name = "Event 1", EventGroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 60, Name = "Event 2", EventGroupId = 1 },
                    new EventEntity { StartDt = 33, EndDt = 40, Name = "Event 3", EventGroupId = 1 },
                    new EventEntity { StartDt = 1, EndDt = 60, Name = "Event 4", EventGroupId = 1 }
                }).ToAsyncEnumerable());


            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IEnumerable<EventGroupEntity>)new[] { new EventGroupEntity { Id = 1, Name = "G1" } }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var sut = new GetEventListByPeriodQueryHandler(mediator, unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }
                , CancellationToken.None))
            {
                item.Should().Match<Event>(m => m.StartDate <= end && m.EndDate >= start
                    && m.Group.Name.Equals("G1", StringComparison.OrdinalIgnoreCase)
                    && (m.Calendar.Year == 2022 || m.Calendar.Year == 2023));
                count++;
            }
            count.Should().Be(6);
        }

        [Fact]
        public async Task Should_GetEventsForWholeYear()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 1, 1);
            DateOnly end = new DateOnly(2022, 12, 31);

            var mediator = Mock.Of<IMediator>();
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<GetCalendarListQuery>(a => a.Year == 2022), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 1, Name = "2022", Year = 2022 }
                }).ToAsyncEnumerable());
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<GetCalendarListQuery>(a => a.Year == 2023), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 2, Name = "2023", Year = 2023 }
                }).ToAsyncEnumerable());

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.Is<int>(a => a == 1), It.Is<int>(a => a == 365)
                    , It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    // Jan 01/Feb 02
                    new EventEntity { StartDt = 1, EndDt = 33, Name = "Event 1", EventGroupId = 1 },
                    // Feb 02/Mar 01
                    new EventEntity { StartDt = 33, EndDt = 60, Name = "Event 2", EventGroupId = 1 },
                    // Feb 02/Feb 09
                    new EventEntity { StartDt = 33, EndDt = 40, Name = "Event 3", EventGroupId = 1 },
                    // Jan 01/Mar 01
                    new EventEntity { StartDt = 1, EndDt = 60, Name = "Event 4", EventGroupId = 1 },
                    // Jul 19/Dec 31
                    new EventEntity { StartDt = 200, EndDt = 365, Name = "Event 5", EventGroupId = 1 },
                    // Oct 10/Dec 26
                    new EventEntity { StartDt = 283, EndDt = 360, Name = "Event 6", EventGroupId = 1 }
                }).ToAsyncEnumerable());

            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IEnumerable<EventGroupEntity>)new[] { new EventGroupEntity { Id = 1, Name = "G1" } }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var sut = new GetEventListByPeriodQueryHandler(mediator, unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }
                , CancellationToken.None))
            {
                item.Should().Match<Event>(m => m.StartDate <= end && m.EndDate >= start
                    && m.Group.Name.Equals("G1", StringComparison.OrdinalIgnoreCase)
                    && m.Calendar.Year == 2022);
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

            var mediator = Mock.Of<IMediator>();
            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                return Mock.Of<IUnitOfWork>();
            };

            var sut = new GetEventListByPeriodQueryHandler(mediator, unitOfWorkFactory);

            // Act 
            Func<Task> func = async () => await sut.Handle(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }
                , CancellationToken.None).FirstAsync();

            // Asserts
            await func.Should().ThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task Should_OperationCancelled_ByRequest()
        {
            // Arrange
            DateOnly start = new DateOnly(2022, 2, 1);
            DateOnly end = new DateOnly(2022, 3, 1);

            var mediator = Mock.Of<IMediator>();
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.IsAny<GetCalendarListQuery>(), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar()
                }).ToAsyncEnumerable());

            var eventRepo = Mock.Of<IEventRepository>();
            Mock.Get(eventRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new EventEntity[0]).ToAsyncEnumerable());

            var eventGroupRepo = Mock.Of<IEventGroupRepository>();
            Mock.Get(eventGroupRepo)
                .Setup(c => c.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult((IEnumerable<EventGroupEntity>)new[] { new EventGroupEntity { Id = 1, Name = "G1" } }));

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.EventRepository)
                    .Returns(eventRepo);
                Mock.Get(uow)
                    .Setup(c => c.EventGroupRepository)
                    .Returns(eventGroupRepo);
                return uow;
            };

            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;

            var sut = new GetEventListByPeriodQueryHandler(mediator, unitOfWorkFactory);

            // Act 
            Func<Task> func = async () => await sut.Handle(new GetEventListByPeriodQuery { StartDate = start, EndDate = end }
                , cancellationToken)
                .GetAsyncEnumerator(cancellationToken)
                .MoveNextAsync();

            source.Cancel();

            // Asserts
            await func.Should().ThrowAsync<OperationCanceledException>();
        }
    }
}