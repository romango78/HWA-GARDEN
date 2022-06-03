using HWA.GARDEN.Utilities.Pipeline;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.CalendarService.Domain.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddMediatR(typeof(DependencyContainer).Assembly)
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(ValidationStreamBehavior<,>))
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(LoggingStreamBehavior<,>));

            Data.Dependencies.DependencyContainer.Init(builder);
        }
    }
}
