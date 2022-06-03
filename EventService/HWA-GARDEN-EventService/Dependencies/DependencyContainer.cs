using Fonlow.DateOnlyExtensions;
using HWA.GARDEN.Contracts.Results;
using HWA.GARDEN.Security;
using HWA.GARDEN.Utilities.Extensions;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.DataProtection;

namespace HWA.GARDEN.EventService.Dependencies
{
    internal static class DependencyContainer
    {
        private const string AzureServiceBusConnectionConfigKey = "AzureServiceBusConnectionString";
        private const string ConnectionStringConfigKey = "DefaultConnectionString";
        private const string DataProtectionApplicationNameConfigKey = "DataProtectionApplicationName";
        private const string DataProtectionPurposeConfigKey = "DataProtectionPurpose";
        private const string DataProtectionStorageFolderConfigKey = "DataProtectionStorageFolder";

        public static void Init(IServiceCollection builder, ConfigurationManager configManager)
        {
            builder.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(
                    Path.Combine(AppContext.BaseDirectory, configManager[DataProtectionStorageFolderConfigKey]))
                ).SetApplicationName(configManager[DataProtectionApplicationNameConfigKey]);
            builder.AddMediatR(typeof(DependencyContainer).Assembly);
            builder.AddMassTransit(config =>
            {
                config.AddRequestClient<CalendarList>();

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
            
            builder.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
                options.SerializerSettings.Converters.Add(new DateOnlyNullableJsonConverter());
            });

            builder.AddExceptionHandlingBasePolicies();
            Domain.Dependencies.DependencyContainer.Init(builder);
        }
    }
}
