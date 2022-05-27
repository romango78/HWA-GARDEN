using FluentAssertions;
using HWA.GARDEN.CalendarService.Comsumers;
using HWA.GARDEN.CalendarService.Domain.Requests;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace HWA.GARDEN.CalendarService.Tests
{
    public class GetCalendarListConsumerTests
    {
        [Fact]
        public async Task ShouldConsum()
        {
            // Arrange
            const int Year = 2022;
            var mediator = Mock.Of<IMediator>();
            Mock.Get(mediator)
                .Setup(c => c.CreateStream(It.Is<CalendarListQuery>(a => a.Year == Year), It.IsAny<CancellationToken>()))
                .Returns((new[]
                {
                    new Calendar { Id = 1, Name = "2022", Year = 2022 },
                    new Calendar { Id = 0, Name = "(default)", Year = 2022 }
                }).ToAsyncEnumerable());

            await using ServiceProvider? provider = SetupServiceProvider(mediator);
            
            ITestHarness? harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            IRequestClient<GetCalendarList>? sut = harness.GetRequestClient<GetCalendarList>();

            // Act
            var result = await sut.GetResponse<CalendarList>(new
            {
                Year = Year
            });

            // Asserts
            (await harness.Sent.Any<CalendarList>()).Should().BeTrue();
            (await harness.Consumed.Any<GetCalendarList>()).Should().BeTrue();

            var consumer = harness.GetConsumerHarness<GetCalendarListConsumer>();
            (await consumer.Consumed.Any<GetCalendarList>()).Should().BeTrue();

            result.Message.Calendars.Should().HaveCount(2)
                .And.OnlyContain(c => c.Year == Year);
        }

        private ServiceProvider SetupServiceProvider(IMediator mediator)
        {

            var provider = new ServiceCollection()
                .AddMassTransitTestHarness(config =>
                {
                    config.AddConsumer<GetCalendarListConsumer>();
                })
                .AddScoped(o => mediator)
                .BuildServiceProvider(true);
            return provider;
        }
    }
}