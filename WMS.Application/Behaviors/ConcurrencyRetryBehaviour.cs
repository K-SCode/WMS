using Mediator;
using WMS.Application.common.Abstraction;
using WMS.Application.common.Exceptions;
using WMS.Application.Interfaces.Infrastructure;
using WMS.Core.Common;

namespace WMS.Application.Behaviors
{
    public class ConcurrencyRetryBehaviour<TMessage, TResponse>(
        IUnitOfWork unitOfWork)
        : IPipelineBehavior<TMessage, TResponse>
        where TMessage : IMessage
        where TResponse : Result, IValidationFailureFactory<TResponse>
    {
        private const int MaxAttempts = 3;
        public async ValueTask<TResponse> Handle(
            TMessage message,
            MessageHandlerDelegate<TMessage, TResponse> next,
            CancellationToken cancellationToken)
        {
            if (message is IVersionedCommand)
                return TResponse.ValidationFailure(Errors.ConcurrencyConflict);

            int i = 1;
            while(true)
            {
                try
                {
                    return await next(message, cancellationToken);
                }
                catch(ConcurrencyConflictException) when (i < MaxAttempts)
                {
                    i++;
                    unitOfWork.ClearTrackedChagnes();
                    await Task.Delay(Random.Shared.Next(20, 100) * i, cancellationToken);
                }
                catch(ConcurrencyConflictException)
                {
                    return TResponse.ValidationFailure(Errors.ConcurrencyConflict);
                }
            }
        }
    }
}
