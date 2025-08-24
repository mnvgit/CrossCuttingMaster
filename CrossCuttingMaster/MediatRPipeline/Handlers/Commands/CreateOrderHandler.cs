using CrossCuttingMaster.MediatRPipeline.Handlers.DomainExeptions;
using CrossCuttingMaster.MediatRPipeline.Handlers.Responses;
using CrossCuttingMaster.MediatRPipeline.Handlers.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace CrossCuttingMaster.MediatRPipeline.Handlers.Commands
{
    // Handler with Transactional attribute to ensure DB transaction
    [Transactional]
    public record CreateOrderCommand : IIdempotentRequest<ApiResponse<Guid>> // returns new OrderId
    {
        public decimal Price { get; init; }
        public int UserId { get; init; }
    }

    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, ApiResponse<Guid>>
    {
        private readonly CreateOrderOptions _options;

        public CreateOrderHandler(IOptions<CreateOrderOptions> options)
        {
            _options = options.Value;
        }

        public async Task<ApiResponse<Guid>> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            try
            {
                if (request.UserId == _options.SpecialUserId)
                {
                    throw new CustomDomainCreateOrderException("CustomDomainCreateOrderException example", CreateOrderErrorCode.PaymentFailed);
                }

                // Pretend DB insert
                await Task.Delay(1, ct);
            }
            catch (Exception ex)
            {
                // Error handling with custom exception
                throw new CustomDomainCreateOrderException(ex.Message, CreateOrderErrorCode.PaymentFailed);
            }
            
            return ApiResponse<Guid>.Ok(Guid.NewGuid());
        }
    }
}
