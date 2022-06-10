using FluentValidation;
using FluentValidation.Results;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HWA.GARDEN.Utilities.Pipeline
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<ValidationBehavior<TRequest, TResponse>> _logger;
        private readonly IEnumerable<IValidator<TRequest>> _validatorList;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validatorList, ILogger<ValidationBehavior<TRequest, TResponse>> logger)
        {
            Requires.NotNull(validatorList, nameof(validatorList));
            Requires.NotNull(logger, nameof(logger));

            _validatorList = validatorList;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validatorList.Any())
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

            return await next();
        }
    }
}
