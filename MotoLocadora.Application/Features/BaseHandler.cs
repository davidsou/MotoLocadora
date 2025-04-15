using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features;

public abstract class BaseHandler(ILogger logger)
{

    protected async Task<OperationResult<T>> TryCatchAsync<T>(Func<Task<OperationResult<T>>> action, string operation)
    {
        try
        {
            logger.LogInformation("Iniciando operação: {Operation}", operation);
            return await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante operação: {Operation}", operation);
            return OperationResult<T>.Failure($"Erro interno ao realizar '{operation}': {ex.Message}");
        }
    }

    protected async Task<OperationResult> TryCatchAsync(Func<Task<OperationResult>> action, string operation)
    {
        try
        {
            logger.LogInformation("Iniciando operação: {Operation}", operation);
            return await action();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro durante operação: {Operation}", operation);
            return OperationResult.Failure($"Erro interno ao realizar '{operation}': {ex.Message}");
        }
    }

}

