using FluentValidation;
using FluentValidation.Results;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.Utilities.Pipeline
{
    public class ValidationStreamBehavior<TRequest, TResponse> : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
    {
        private readonly ILogger<ValidationStreamBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validatorList;

        public ValidationStreamBehavior(IEnumerable<IValidator<TRequest>> validatorList, ILogger<ValidationStreamBehavior<TRequest, TResponse>> logger)
        {
            Requires.NotNull(validatorList, nameof(validatorList));
            Requires.NotNull(logger, nameof(logger));

            _validatorList = validatorList;
            _logger = logger;
        }

        public async IAsyncEnumerable<TResponse> Handle(TRequest request
            , [EnumeratorCancellation] CancellationToken cancellationToken
            , StreamHandlerDelegate<TResponse> next)
        {
            if(_validatorList.Any())
            {
                _logger.LogInformation($"Validating {typeof(TRequest).Name} request. {_validatorList.Count()} validators was found...");

                ValidationResult[]? validationResults = 
                    await Task.WhenAll(_validatorList.Select(m => m.ValidateAsync(request, cancellationToken)))
                    .ConfigureAwait(false);
                ValidationFailure[]? failures = 
                    validationResults.SelectMany(p => p.Errors).Where(c => c != null)
                    .ToArray();
                if (failures.Length != 0)
                {
                    _logger.LogWarning($"{typeof(TRequest).Name} request failed...\r\n{string.Join("\r\n", failures.Select(p => p.ErrorMessage))}");
                    throw new ValidationException(failures);
                }
            }

            await foreach (var response in next()
                .WithCancellation(cancellationToken)
                .ConfigureAwait(false))
            {
                yield return response;
            }
        }
    }
}
