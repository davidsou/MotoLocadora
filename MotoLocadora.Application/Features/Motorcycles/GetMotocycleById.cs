using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Application.Features.Motorcycles.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;

public class GetMotorcycleById
{
    public record Query(int Id) : IRequest<OperationResult<MotorcycleDto>>;

    public class Handler(IMotorcycleRepository repository, ILogger<GetMotorcycleById.Handler> logger) 
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<MotorcycleDto>>
    {
        public async Task<OperationResult<MotorcycleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult<MotorcycleDto>.Failure("Motocicleta não encontrada.");

                return OperationResult<MotorcycleDto>.Success(entity.ToDto());
            }, "Buscar motocicleta por ID");
        }
    }
}

