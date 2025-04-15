using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.BuildingBlocks.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse>(ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<ExceptionHandlingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception for Request {RequestName}: {@Request}", typeof(TRequest).Name, request);

            // Se for do tipo OperationResult<TResponse>, encapsula o erro
            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(OperationResult<>))
            {
                var failureMethod = typeof(OperationResult<>)
                    .MakeGenericType(typeof(TResponse).GetGenericArguments().First())
                    .GetMethod("Failure", new[] { typeof(string[]) });

                if (failureMethod != null)
                {
                    var result = (TResponse)failureMethod.Invoke(null, new object[] { new[] { ex.Message } })!;
                    return result;
                }
            }

            throw; // Se não conseguir encapsular, lança a exceção normalmente
        }
    }
}
