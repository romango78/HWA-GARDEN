using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.RabbitMQ.Extensions
{
    public static class RabbitMqExtension
    {
        public static void AddMassTransitAndConfig(this IServiceCollection services,
            Uri rabbitMqUri, string userName, string password,
            Action<IBusRegistrationConfigurator> addConsumer = null)
        {
            services.AddMassTransit(config =>
            {
                if (addConsumer != null)
                {
                    addConsumer?.Invoke(config);
                }
                
                config.UsingRabbitMq((context, rmConfig) =>
                {
                    rmConfig.Host(rabbitMqUri, hostCconfig =>
                    {
                        hostCconfig.Username(userName);
                        hostCconfig.Password(password);
                    });
                    rmConfig.ConfigureEndpoints(context);
                });
            });
        }
    }
}
