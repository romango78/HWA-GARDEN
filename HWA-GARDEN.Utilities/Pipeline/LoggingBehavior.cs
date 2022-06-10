using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HWA.GARDEN.Utilities.Pipeline
{
    public sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            Requires.NotNull(logger, nameof(logger));

            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name} request...");

            var response = await next();

            _logger.LogInformation($"{typeof(TRequest).Name} request was handled. {typeof(TResponse).Name} has been received... ");

            return response;
        }
    }
}
