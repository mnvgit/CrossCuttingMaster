using MediatR;
using System.Transactions;

namespace CrossCuttingMaster.MediatRPipeline.Handlers.Settings
{
    // Why use it?
    // Without it, every single request going through MediatR would be wrapped in a TransactionScope.
    // That means even queries (read-only requests) or commands that don’t need DB changes would pay the transaction overhead. 
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class TransactionalAttribute : Attribute { }

    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            // Only wrap if [Transactional] is applied
            if (!request.GetType().IsDefined(typeof(TransactionalAttribute), inherit: true))
                return await next(ct);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var response = await next(ct);

                // commit
                scope.Complete();
                return response;
            }
            catch
            {
                scope.Dispose();
                throw;
            }
        }
    }
}
