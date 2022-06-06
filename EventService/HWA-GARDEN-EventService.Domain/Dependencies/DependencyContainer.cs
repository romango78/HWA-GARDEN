﻿using FluentValidation;
using HWA.GARDEN.Utilities.Pipeline;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace HWA.GARDEN.EventService.Domain.Dependencies
{
    public static class DependencyContainer
    {
        public static void Init(IServiceCollection builder)
        {
            builder.AddValidatorsFromAssembly(typeof(DependencyContainer).Assembly)
                .AddMediatR(typeof(DependencyContainer).Assembly)
                .AddAutoMapper(typeof(DependencyContainer).Assembly)
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(ValidationStreamBehavior<,>))
                .AddScoped(typeof(IStreamPipelineBehavior<,>), typeof(LoggingStreamBehavior<,>));

            Data.Dependencies.DependencyContainer.Init(builder);            
        }
    }
}
