using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace MotoLocadora.BuildingBlocks.Behaviors;

public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next();

        stopwatch.Stop();

        var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;

        if (elapsedMilliseconds > 500) // Tempo limite que você achar razoável
        {
            _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} ms) {@Request}",
                typeof(TRequest).Name, elapsedMilliseconds, request);
        }
        else
        {
            _logger.LogInformation("Request handled: {Name} ({ElapsedMilliseconds} ms)", typeof(TRequest).Name, elapsedMilliseconds);
        }

        return response;
    }
}
