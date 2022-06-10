using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Xunit.Abstractions;
using Fonlow.DateOnlyExtensions;

namespace HWA.GARDEN.Tests.Utilities
{
    public class StateMachineTestFixture<TStateMachine, TInstance> : IAsyncDisposable
        where TStateMachine : class, SagaStateMachine<TInstance>
        where TInstance : class, SagaStateMachineInstance
    {
        private Task<IScheduler> _scheduler;
        private TimeSpan _testOffset;

        public StateMachineTestFixture(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;

            InterceptQuartzSystemTime();

            var collection = new ServiceCollection()
                .AddLogging((builder) => builder.AddXUnit(OutputHelper))
                .AddMassTransitTestHarness(config =>
                {
                    config.SetKebabCaseEndpointNameFormatter();

                    config.AddSagaStateMachine<TStateMachine, TInstance>()
                        .InMemoryRepository();

                    config.AddPublishMessageScheduler();

                    ConfigureMassTransit(config);

                    config.UsingInMemory((context, cfg) =>
                    {
                        cfg.UseInMemoryScheduler(out _scheduler);
                        cfg.ConfigureEndpoints(context);
                    });
                });

            ConfigureServices(collection);

            Provider = collection.BuildServiceProvider(true);

            var loggerFactory = Provider.GetRequiredService<ILoggerFactory>();
            Quartz.Logging.LogContext.SetCurrentLogProvider(loggerFactory);

            TestHarness = Provider.GetTestHarness();
        }

        protected ITestOutputHelper OutputHelper { get; }

        protected ServiceProvider Provider { get; }

        protected ITestHarness TestHarness { get; }

        protected async Task AdvanceSystemTime(TimeSpan duration)
        {
            if (duration <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration));

            var scheduler = await _scheduler.ConfigureAwait(false);

            await scheduler.Standby().ConfigureAwait(false);

            _testOffset += duration;

            await scheduler.Start().ConfigureAwait(false);
        }

        protected virtual void ConfigureMassTransit(IBusRegistrationConfigurator configurator)
        {
        }

        protected virtual void ConfigureServices(IServiceCollection collection)
        {
        }

        private void InterceptQuartzSystemTime()
        {
            SystemTime.UtcNow = GetUtcNow;
            SystemTime.Now = GetNow;
        }

        private DateTimeOffset GetUtcNow()
        {
            return DateTimeOffset.UtcNow + _testOffset;
        }

        private DateTimeOffset GetNow()
        {
            return DateTimeOffset.Now + _testOffset;
        }

        private static void RestoreDefaultQuartzSystemTime()
        {
            SystemTime.UtcNow = () => DateTimeOffset.UtcNow;
            SystemTime.Now = () => DateTimeOffset.Now;
        }

        protected virtual async ValueTask DisposeAsyncCore()
        {
            await Provider.DisposeAsync().ConfigureAwait(false);

            RestoreDefaultQuartzSystemTime();
        }

        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore().ConfigureAwait(false);

            GC.SuppressFinalize(this);
        }
    }
}
