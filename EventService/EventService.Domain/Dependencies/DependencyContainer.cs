using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.EventService.Domain.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(DependencyContainer));

            Data.Dependencies.DependencyContainer.Init(builder);            
        }
    }
}
