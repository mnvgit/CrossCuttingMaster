using CrossCuttingMaster.Services.AuditService;
using MediatR;
using System.Text.Json;

namespace CrossCuttingMaster.MediatRPipeline.Behaviors
{
    // This behavior logs audit information for each request and response in the MediatR pipeline.
    // It captures the request type, response type, timestamp, and serialized data.
    public class AuditLogBehavior<TRequest, TResponse>(
        IAuditLogger auditLogger
    ) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            AuditLog log = new()
            {
                RequestType = typeof(TRequest).Name,
                ResponseType = typeof(TResponse).Name,
                Timestamp = DateTime.UtcNow,
                RequestData = JsonSerializer.Serialize(request)
            };

            try
            {
                var response = await next(ct);
                log.ResponseData = JsonSerializer.Serialize(response);
                await auditLogger.LogAsync(log);
                return response;
            }
            catch (Exception ex)
            {
                log.ResponseData = JsonSerializer.Serialize(new { Error = ex.Message, Exception = ex.ToString() });
                await auditLogger.LogAsync(log);
                throw;
            }
        }
    }
}