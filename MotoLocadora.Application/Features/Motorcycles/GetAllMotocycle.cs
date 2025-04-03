using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Application.Features.Motorcycles.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;

public class GetAllMotorcycles
{
    public record Query() : IRequest<OperationResult<List<MotorcycleDto>>>;

    public class Handler(IMotorcycleRepository repository, ILogger<GetAllMotorcycles.Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<List<MotorcycleDto>>>
    {
        public async Task<OperationResult<List<MotorcycleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var list = await repository.GetAllAsync();
                var dtos = list.Select(x => x.ToDto()).ToList();
                return OperationResult<List<MotorcycleDto>>.Success(dtos);
            }, "Listar motocicletas");
        }
    }
}

