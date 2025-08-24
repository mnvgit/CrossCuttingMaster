using CrossCuttingMaster.MediatRPipeline.Handlers.Commands;
using CrossCuttingMaster.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CrossCuttingMaster.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost(Name = "CreateOrder")]
        public async Task<ApiResponse<Guid>> Post(CreateOrderCommand command, CancellationToken ct)
        {
            return await _mediator.Send(command, ct);
        }
    }
}
