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

            int i = 1;
            while(true)
            {
                try
                {
                    return await next(message, cancellationToken);
                }
                catch(ConcurrencyConflictException) when (i < MaxAttempts && message is not IVersionedCommand)
                {
                    i++;
                    unitOfWork.ClearTrackedChanges();
                    await Task.Delay(Random.Shared.Next(20, 100) * i, cancellationToken);
                }
                catch(ConcurrencyConflictException)
                {
                    unitOfWork.ClearTrackedChanges();
                    return TResponse.ValidationFailure(Errors.ConcurrencyConflict);
                }
            }
        }
    }
}
