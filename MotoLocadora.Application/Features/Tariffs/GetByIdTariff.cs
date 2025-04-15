using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;

public class GetTariffById
{
    public record Query(int Id) : IRequest<OperationResult<TariffDto>>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<TariffDto>>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult<TariffDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult<TariffDto>.Failure("Tarifa não encontrada.");

                return OperationResult<TariffDto>.Success(entity.ToDto());
            }, "Buscar tarifa por ID");
        }
    }
}

