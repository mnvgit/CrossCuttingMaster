using MediatR;

namespace CrossCuttingMaster.MediatRPipeline.Handlers.Settings
{
    // Interface for commands that support idempotency
    public interface IIdempotentRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
