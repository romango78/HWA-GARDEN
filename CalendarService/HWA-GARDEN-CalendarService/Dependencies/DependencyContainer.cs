using HWA.GARDEN.Security;
using HWA.GARDEN.CalendarService.Comsumers;
using MediatR;
using MassTransit;
using Microsoft.AspNetCore.DataProtection;
using System.Security.Cryptography;

namespace HWA.GARDEN.CalendarService.Dependencies
{
    public static class DependencyContainer
    {
        private const string AzureServiceBusConnectionConfigKey = "AzureServiceBusConnectionString";        
        private const string ConnectionStringConfigKey = "DefaultConnectionString";
        private const string DataProtectionApplicationNameConfigKey = "DataProtectionApplicationName";
        private const string DataProtectionPurposeConfigKey = "DataProtectionPurpose";
        private const string DataProtectionStorageFolderConfigKey = "DataProtectionStorageFolder";
        private const string GetCalendarListQueueConfigKey = "GetCalendarListQueueName";

        public static void Init(IServiceCollection builder, ConfigurationManager configManager)
        {
            builder.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(
                    Path.Combine(AppContext.BaseDirectory, configManager[DataProtectionStorageFolderConfigKey]))
                ).SetApplicationName(configManager[DataProtectionApplicationNameConfigKey]);
            builder.AddMediatR(typeof(DependencyContainer));
            builder.AddMassTransit(config =>
            {
                config.AddConsumer<GetCalendarListConsumer>();

                config.SetKebabCaseEndpointNameFormatter();

                config.UsingAzureServiceBus((context, azureConfig) =>
                {
                    azureConfig.Host(configManager.GetConnectionString(AzureServiceBusConnectionConfigKey));

                    azureConfig.ReceiveEndpoint(configManager[GetCalendarListQueueConfigKey], endpointConfig =>
                    {
                        endpointConfig.UseMessageRetry(retryConfig =>
                        {
                            retryConfig.Immediate(5);
                            retryConfig.Ignore<CryptographicException>();
                        });
                        endpointConfig.ConfigureConsumer<GetCalendarListConsumer>(context);
                    });
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
