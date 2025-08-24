using CrossCuttingMaster.MediatRPipeline.Handlers.Commands;
using CrossCuttingMaster.Responses;
using FluentValidation;
using MediatR;

namespace CrossCuttingMaster.MediatRPipeline.Behaviors
{
    // This behavior handles unhandled exceptions in the MediatR pipeline.
    // It catches exceptions, logs them, and returns a standardized error response.
    public class UnhandledExceptionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    {
        private readonly ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UnhandledExceptionBehavior(ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            var httpContext = _httpContextAccessor.HttpContext!;

            try
            {
                return await next(ct);
            }
            catch (Exception ex)
            {
                ApiResponse<TResponse> res;
                switch (ex)
                {
                    case ValidationException validationEx:
                        _logger.LogError(validationEx, "Validation failed for request {RequestName}", typeof(TRequest).Name);
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        List<string> validationErrors = new List<string>();
                        foreach (var error in validationEx.Errors)
                        {
                            validationErrors.Add(error.ErrorMessage);
                        }
                        res = ApiResponse<TResponse>.Fail(StatusCodes.Status400BadRequest, validationErrors);
                        break;

                    case CustomDomainCreateOrderException createOrderEx:
                        _logger.LogError(createOrderEx, "Failed to create order for request {RequestName}", typeof(TRequest).Name);
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        res = ApiResponse<TResponse>.Fail((int)createOrderEx.Code, [createOrderEx.Message]);
                        break;

                    default:
                        _logger.LogError(ex, "An unhandled exception occurred for request {RequestName}", typeof(TRequest).Name);
                        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        res = ApiResponse<TResponse>.Fail(StatusCodes.Status500InternalServerError, ["An unexpected error occurred."]);
                        break;
                }

                await httpContext.Response.WriteAsJsonAsync(res, ct).ConfigureAwait(false);
                throw;
            }
        }
    }
}
