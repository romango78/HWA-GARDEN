using Community.AspNetCore.ExceptionHandling;
using Community.AspNetCore.ExceptionHandling.NewtonsoftJson;
using FluentValidation;
using HWA.GARDEN.Utilities.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

namespace HWA.GARDEN.Utilities.Extensions
{
#pragma warning disable CS0618 // Type or member is obsolete
    public static class ExceptionHandlingExtension
    {
        public static IServiceCollection AddExceptionHandlingBasePolicies(this IServiceCollection services)
        {
            return services.AddExceptionHandlingBasePolicies(null);
        }

        public static IServiceCollection AddExceptionHandlingBasePolicies(this IServiceCollection services, Action<IExceptionPolicyBuilder>? builder)
        {
            services.AddExceptionHandlingPolicies(policy =>
            {
                builder?.Invoke(policy);

                policy.For<ValidationException>()
                    .Response(statusCodeFactory: f => StatusCodes.Status400BadRequest)
                    .WithBodyJson((request, exception) => new
                    {
                        errors = exception.Errors
                            .GroupBy(p => p.PropertyName)
                            .Select(p => new { Key = p.Key, ErrorList = p.Select(p2 => p2.ErrorMessage) })
                            .ToDictionary(s => s.Key, s => s.ErrorList),
                        type = Strings.GetString("http:bad-request"),
                        title = Strings.GetString("exception-title:validation"),
                        status = StatusCodes.Status400BadRequest
                    })
                    .NextPolicy();

                policy.For<Exception>()
                    .Response(statusCodeFactory: f => StatusCodes.Status500InternalServerError
                        , responseAlreadyStartedBehaviour: ResponseAlreadyStartedBehaviour.GoToNextHandler)
                        .ClearCacheHeaders()
                        .Headers((headers, exception) =>
                        {
                            headers[HeaderNames.ContentType] = MediaTypeNames.Application.Json;
                        })
                        .WithBodyJson((request, exception) => new
                        {
                            exception = new
                            {
                                source = exception.GetType().FullName,
                                message = exception.Message,
                                stackTrace = exception.StackTrace
                            },
                            type = Strings.GetString("http:internal-server-error"),
                            title = Strings.GetString("exception-title:general"),
                            status = StatusCodes.Status500InternalServerError
                        })
                    .Handled();
            });

            return services;
        }
    }
#pragma warning restore CS0618 // Type or member is obsolete
}
