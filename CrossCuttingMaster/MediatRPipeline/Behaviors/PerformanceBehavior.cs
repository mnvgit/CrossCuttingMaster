using CrossCuttingMaster.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace CrossCuttingMaster.MediatRPipeline.Behaviors
{
    // This behavior measures the execution time of each request in the MediatR pipeline.
    // If the execution time exceeds defined milliseconds, it logs a warning.
    public class PerformanceBehavior<TRequest, TResponse>(
        ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
        IOptions<PerformanceBehaviorSettings> options) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
        private readonly Stopwatch _timer = new();
        private readonly int _threshold = options.Value.ThresholdMilliseconds;

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct)
        {
            _timer.Start(); // Start tracking execution time

            var response = await next(ct); // Execute the next behavior or request handler

            _timer.Stop(); // Stop the timer

            if (_timer.ElapsedMilliseconds > _threshold) // Log if execution time exceeds threshold
            {
                _logger.LogWarning(
                    "{RequestName} took {ElapsedMilliseconds}ms to execute",
                    typeof(TRequest).Name,
                    _timer.ElapsedMilliseconds);
            }

            return response;
        }
    }
}
