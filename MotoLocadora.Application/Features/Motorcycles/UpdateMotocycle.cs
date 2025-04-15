using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;


public class UpdateMotorcycle
{
    public record Command(int Id, MotorcycleDto Motorcycle) : IRequest<OperationResult<Unit>>;

    public class Handler(IMotorcycleRepository repository, ILogger<UpdateMotorcycle.Handler> logger) : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Motocicleta não encontrada.");

                entity.Ano = request.Motorcycle.Ano;
                entity.Modelo = request.Motorcycle.Modelo;
                entity.Placa = request.Motorcycle.Placa;
                entity.ChangedAt = DateTime.UtcNow;

                await repository.UpdateAsync(entity);
                return OperationResult<Unit>.Success();
            }, "Atualizar motocicleta");
        }
    }
}
