using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Application.Features.Motorcycles.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Domain.Entities;
using System.Linq.Expressions;

namespace MotoLocadora.Application.Features.Motorcycles;

public class QueryMotorcycleById
{
    public record Query(int Id, Expression<Func<Domain.Entities.Motorcycle, object>>[]? Includes = null) : IRequest<OperationResult<MotorcycleDto>>;

    public class Handler(IMotorcycleRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<MotorcycleDto>>
    {
        private readonly IMotorcycleRepository _repository = repository;

        public async Task<OperationResult<MotorcycleDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var includes = request.Includes;

                var items = await _repository.FindWithIncludesAsync(m => m.Id == request.Id, includes);
                var entity = items.FirstOrDefault();

                if (entity == null)
                    return OperationResult<MotorcycleDto>.Failure("Motocicleta não encontrada.");

                return OperationResult<MotorcycleDto>.Success(entity.ToDto());
            }, "Consulta detalhada de motocicleta");
        }
    }
}
