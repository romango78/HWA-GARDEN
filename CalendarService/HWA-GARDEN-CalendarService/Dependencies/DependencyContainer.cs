using HWA.GARDEN.Security;
using HWA.GARDEN.CalendarService.Comsumers;
using MediatR;
using MassTransit;
using Microsoft.AspNetCore.DataProtection;

namespace HWA.GARDEN.CalendarService.Dependencies
{
    public static class DependencyContainer
    {
        private const string AzureServiceBusConnectionConfigKey = "AzureServiceBusConnectionString";        
        private const string ConnectionStringConfigKey = "DefaultConnectionString";
        private const string DataProtectionPurposeConfigKey = "DataProtectionPurpose";
        private const string GetCalendarListQueueConfigKey = "GetCalendarListQueueName";

        public static void Init(IServiceCollection builder, ConfigurationManager configManager)
        {
            builder.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "keys")))
                .SetApplicationName("HWA-GARDEN");
            builder.AddMediatR(typeof(DependencyContainer));
            builder.AddMassTransit(config =>
            {
                config.AddConsumer<GetCalendarListConsumer>()
                        .Endpoint(config => config.Name = configManager[GetCalendarListQueueConfigKey]);

                config.UsingAzureServiceBus((context, azureConfig) =>
                {
                    azureConfig.Host(configManager.GetConnectionString(AzureServiceBusConnectionConfigKey));

                    azureConfig.ConfigureEndpoints(context);
                });
            });
            
            builder.AddScoped<ISecurityContext, SecurityContext>((config) =>
            {
                IDataProtectionProvider? dataProtectionProvider = config.GetRequiredService<IDataProtectionProvider>();
                return new SecurityContext(dataProtectionProvider, 
                    configManager.GetConnectionString(ConnectionStringConfigKey),
                    configManager[DataProtectionPurposeConfigKey]);
            });
            
            Domain.Dependencies.DependencyContainer.Init(builder);
        }
    }
}
