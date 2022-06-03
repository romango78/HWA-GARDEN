using FluentValidation;
using FluentValidation.Results;
using HWA.GARDEN.Utilities.Validation;
using MediatR;
using System.Runtime.CompilerServices;

namespace HWA.GARDEN.Utilities.Pipeline
{
    public class ValidationStreamBehavior<TRequest, TResponse> : IStreamPipelineBehavior<TRequest, TResponse>
        where TRequest : IStreamRequest<TResponse>
    {
        public readonly IEnumerable<IValidator<TRequest>> _validatorList;

        public ValidationStreamBehavior(IEnumerable<IValidator<TRequest>> validatorList)
        {
            Requires.NotNull(validatorList, nameof(validatorList));

            _validatorList = validatorList;
        }

        public async IAsyncEnumerable<TResponse> Handle(TRequest request
            , [EnumeratorCancellation] CancellationToken cancellationToken
            , StreamHandlerDelegate<TResponse> next)
        {
            if(_validatorList.Any())
            {
                await Task.WhenAll(_validatorList.Select(m => m.ValidateAndThrowAsync(request, cancellationToken)))
                    .ConfigureAwait(false);
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
