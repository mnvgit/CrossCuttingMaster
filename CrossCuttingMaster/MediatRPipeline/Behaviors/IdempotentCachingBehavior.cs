using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace CrossCuttingMaster.MediatRPipeline.Behaviors
{
    // Caches responses for idempotent requests based on an Idempotency-Key header.
    // If a request with the same key is received again, the cached response or exception is returned.
    public class IdempotentCachingBehavior<TRequest, TResponse>(IHttpContextAccessor httpContextAccessor, IMemoryCache cache) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _cache = cache;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            if (request is not IIdempotentRequest<TResponse> idempotentRequest)
                return await next(ct);

            // Try to read header
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext is null)
                throw new InvalidOperationException("No HttpContext available");

            // Check for Idempotency-Key header
            if (!httpContext.Request.Headers.TryGetValue("Idempotency-Key", out var idempotencyKeyFromHeader)
                || string.IsNullOrWhiteSpace(idempotencyKeyFromHeader))
            {
                throw new InvalidOperationException("Missing Idempotency-Key header");
            }

            if (_cache.TryGetValue(idempotencyKeyFromHeader, out var cachedResponse))
            {
                if (cachedResponse is Exception ex)
                    throw ex; // replay the same failure
                return (TResponse)cachedResponse!;
            }

            try
            {
                var response = await next(ct);

                _cache.Set(idempotencyKeyFromHeader, response,
                    new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });

                return response;
            }
            catch (Exception ex)
            {
                // cache exception too, to replay same failure on retries
                _cache.Set(idempotencyKeyFromHeader, ex,
                    new MemoryCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(10) });
                throw;
            }
        }
    }
}
