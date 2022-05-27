using HWA.GARDEN.AzureServiceBus.Extensions;
using HWA.GARDEN.Security;
using HWA.GARDEN.CalendarService.Comsumers;
using MediatR;

namespace HWA.GARDEN.CalendarService.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder, string connectionString, 
            string serviceBusConnectionString)
        {
            builder.AddScoped<ISecurityContext, SecurityContext>((config) =>
            {
                return new SecurityContext(connectionString);
            });

            Domain.Dependencies.DependencyContainer.Init(builder);
                        
            builder.AddMediatR(typeof(DependencyContainer));

            builder.AddMassTransitAndConfig(serviceBusConnectionString,
                addConsumer: config =>
                {
                    config.AddConsumer<GetCalendarListConsumer>()
                        .Endpoint(config => config.Name = "get-calendar-list-queue");
                });
        }
    }
}
