using Fonlow.DateOnlyExtensions;
using HWA.GARDEN.Security;

namespace HWA.GARDEN.EventService.Dependencies
{
    internal static class DependencyContainer
    {
        public static void Init(IServiceCollection builder, string connectionString)
        {            
            builder.AddScoped<ISecurityContext, SecurityContext>((config) =>
            {
                return new SecurityContext(connectionString);
            });

            Domain.Dependencies.DependencyContainer.Init(builder);
            
            builder.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.DateParseHandling = Newtonsoft.Json.DateParseHandling.DateTimeOffset;
                options.SerializerSettings.Converters.Add(new DateOnlyJsonConverter());
                options.SerializerSettings.Converters.Add(new DateOnlyNullableJsonConverter());
            });
        }
    }
}
