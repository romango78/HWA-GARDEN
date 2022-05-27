using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.AzureServiceBus.Extensions
{
    public static class AzureServiceBusExtension
    {
        public static void AddMassTransitAndConfig(this IServiceCollection services,
            string connectionString, Action<IBusRegistrationConfigurator> addConsumer = null)
        {
            services.AddMassTransit(config =>
            {
                if (addConsumer != null)
                {
                    addConsumer?.Invoke(config);
                }

                config.UsingAzureServiceBus((context, azureConfig) =>
                {
                    azureConfig.Host(connectionString);

                    azureConfig.ConfigureEndpoints(context);
                });
            });
        }
    }
}
