using MediatR;

namespace CrossCuttingMaster.MediatRPipeline.Behaviors
{
    // Interface for commands that support idempotency
    public interface IIdempotentRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
