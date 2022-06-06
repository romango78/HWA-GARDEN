using FluentAssertions;
using HWA.GARDEN.Contracts;
using HWA.GARDEN.Contracts.Messages;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.EventService.Domain.Handlers;
using HWA.GARDEN.EventService.Domain.Requests;
using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace HWA.GARDEN.EventService.Domain.Tests.Handlers
{
    [Trait("TestCategory", "UnitTest")]
    public class GetCalendarListQueryHandlerTests
    {
        [Fact]
        public async Task Should_GetCalendarsForSpecifiedYear()
        {
            // Arrange
            const int TestYear = 2022;

            IConsumer<GetCalendarList> consumer = Mock.Of<IConsumer<GetCalendarList>>();
            Mock.Get(consumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<GetCalendarList>>()))
                .Callback<ConsumeContext<GetCalendarList>>((context) => 
                {
                    context.RespondAsync<CalendarList>(new
                    {
                        Calendars = new[]
                        {
                            new
                            {
                                Name = TestYear.ToString(),
                                Year = TestYear
                            },
                            new
                            {
                                Name = "(default)",
                                Year = TestYear
                            }
                        }
                    });
                });

            await using ServiceProvider? provider = SetupServiceProvider(consumer);
            ITestHarness? harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            IRequestClient<GetCalendarList>? client = harness.GetRequestClient<GetCalendarList>();

            GetCalendarListQueryHandler? sut = new GetCalendarListQueryHandler(client);

            // Act & Asserts
            int count = 0;
            await foreach (var item in sut.Handle(new GetCalendarListQuery { Year = TestYear }
                , CancellationToken.None))
            {
                item.Should().Match<Calendar>(m => m.Year == TestYear);
                count++;
            }
            count.Should().Be(2);
        }

        [Fact]
        public async Task Should_OperationCancelled_ByRequest()
        {
            // Arrange
            IConsumer<GetCalendarList> consumer = Mock.Of<IConsumer<GetCalendarList>>();
            Mock.Get(consumer)
                .Setup(c => c.Consume(It.IsAny<ConsumeContext<GetCalendarList>>()))
                .Callback<ConsumeContext<GetCalendarList>>((context) =>
                {
                    context.RespondAsync<CalendarList>(new
                    {
                        Calendars = new Calendar[0]
                    });
                });

            await using ServiceProvider? provider = SetupServiceProvider(consumer);
            ITestHarness? harness = provider.GetRequiredService<ITestHarness>();
            await harness.Start();

            IRequestClient<GetCalendarList>? client = harness.GetRequestClient<GetCalendarList>();

            var source = new CancellationTokenSource();
            var cancellationToken = source.Token;

            GetCalendarListQueryHandler? sut = new GetCalendarListQueryHandler(client);

            // Act 
            Func<Task> func = async () => await sut.Handle(new GetCalendarListQuery { Year = 2022 }
                , cancellationToken)
                .GetAsyncEnumerator(cancellationToken)
                .MoveNextAsync();

            source.Cancel();
            // Asserts
            await func.Should().ThrowAsync<OperationCanceledException>();
        }

        private ServiceProvider SetupServiceProvider(IConsumer<GetCalendarList> consumer)
        {

            var provider = new ServiceCollection()
                .AddScoped(o => consumer)
                .AddMassTransitTestHarness(config =>
                {
                    config.AddConsumer<IConsumer<GetCalendarList>>();
                })
                .BuildServiceProvider(true);
            return provider;
        }
    }
}
