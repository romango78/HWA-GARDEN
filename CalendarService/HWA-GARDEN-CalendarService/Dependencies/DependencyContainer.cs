using HWA.GARDEN.Common.Security;
using MediatR;

namespace HWA.GARDEN.CalendarService.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder, string connectionString)
        {
            builder.AddScoped<ISecurityContext, SecurityContext>((config) =>
            {
                return new SecurityContext(connectionString);
            });

            Domain.Dependencies.DependencyContainer.Init(builder);
                        
            builder.AddMediatR(typeof(DependencyContainer));
        }
    }
}
