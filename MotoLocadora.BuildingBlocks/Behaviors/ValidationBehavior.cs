using FluentValidation;
using MediatR;
using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            // Retorna OperationResult Failure
            var errorMessages = failures.Select(e => e.ErrorMessage).ToArray();

            if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(OperationResult<>))
            {
                var failureMethod = typeof(OperationResult<>)
                    .MakeGenericType(typeof(TResponse).GetGenericArguments().First())
                    .GetMethod("Failure", new[] { typeof(string[]) });

                if (failureMethod != null)
                {
                    var result = (TResponse)failureMethod.Invoke(null, new object[] { errorMessages })!;
                    return result;
                }
            }

            throw new ValidationException(failures);
        }

        return await next();
    }
}
