using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;


public class DeleteMotorcycle
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler(IMotorcycleRepository repository, ILogger<DeleteMotorcycle.Handler> logger) 
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Motocicleta não encontrada.");

                await repository.DeleteAsync(entity);
                return OperationResult.Success();
            }, "Deletar motocicleta");
        }
    }
}