using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.EventService.Domain.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            Data.Dependencies.DependencyContainer.Init(builder);

            builder.AddScoped<IEventService, EventService>();
        }
    }
}
