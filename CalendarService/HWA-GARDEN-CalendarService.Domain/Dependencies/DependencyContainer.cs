using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.CalendarService.Domain.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            Data.Dependencies.DependencyContainer.Init(builder);
                        
            builder.AddMediatR(typeof(DependencyContainer));
        }
    }
}
