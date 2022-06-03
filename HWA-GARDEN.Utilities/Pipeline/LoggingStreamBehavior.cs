using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.Utilities.Pipeline
{
    public sealed class LoggingStreamBehavior<TRequest, TResponse> : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
    {
        private readonly ILogger<LoggingStreamBehavior<TRequest, TResponse>> _logger;

        public LoggingStreamBehavior(ILogger<LoggingStreamBehavior<TRequest, TResponse>> logger)
        {
            Requires.NotNull(logger, nameof(logger));

            _logger = logger;
        }

        public async IAsyncEnumerable<TResponse> Handle(TRequest request, [EnumeratorCancellation] CancellationToken cancellationToken,
            StreamHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation($"Handling {typeof(TRequest).Name} request...");

            int count = 0;
            await foreach (var response in next()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                count++;
                yield return response;
            }

            _logger.LogInformation($"{typeof(TRequest).Name} request was handled. Data [count:{count}] was received... ");
        }
    }
}
