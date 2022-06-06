using AutoMapper;
using FluentAssertions;
using HWA.GARDEN.CalendarService.Data;
using HWA.GARDEN.CalendarService.Data.Entities;
using HWA.GARDEN.CalendarService.Data.Repositories;
using HWA.GARDEN.CalendarService.Domain.Handlers;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using Moq;

namespace HWA.GARDEN.CalendarService.Domain.Tests
{
    public class CalendarListQueryHandlerTests
    {
        [Fact]
        public async Task Should_GetCalendarList_WhenCalendarExistsInDb()
        {
            // Arrange
            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetListAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new CalendarEntity {Id = 1, Name = "Calendar", Description = "Desc", Year = 2022 }
                }).ToAsyncEnumerable());

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                return uow;
            };

            var sut = new CalendarListQueryHandler(GetMapper(), unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new CalendarListQuery { Year = 2022 }
                , CancellationToken.None))
            {
                item.Should().Match<Calendar>(m => !string.IsNullOrEmpty(m.Name) && !string.IsNullOrEmpty(m.Description) && m.Year == 2022);
                count++;
            }
            count.Should().Be(2);
        }

        [Fact]
        public async Task Should_GetCalendarList_WhenCalendarDoesNotExistInDb()
        {
            // Arrange
            var calendarRepo = Mock.Of<ICalendarRepository>();
            Mock.Get(calendarRepo)
                .Setup(c => c.GetListAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .Returns((new CalendarEntity[0]).ToAsyncEnumerable());

            Func<IUnitOfWork> unitOfWorkFactory = () =>
            {
                var uow = Mock.Of<IUnitOfWork>();
                Mock.Get(uow)
                    .Setup(c => c.CalendarRepository)
                    .Returns(calendarRepo);
                return uow;
            };

            var sut = new CalendarListQueryHandler(GetMapper(), unitOfWorkFactory);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new CalendarListQuery { Year = 2022 }
                , CancellationToken.None))
            {
                item.Should().Match<Calendar>(m => m.Id == 0 && m.Name == "(default)" && !string.IsNullOrEmpty(m.Description) 
                    && m.Year == 2022);
                count++;
            }
            count.Should().Be(1);
        }

        private IMapper GetMapper()
        {
            var mapperCfg = new MapperConfiguration(cfg => cfg.AddMaps(typeof(CalendarListQueryHandler).Assembly));
            return mapperCfg.CreateMapper();
        }
    }
}