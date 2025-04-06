using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using System.Linq.Expressions;

namespace MotoLocadora.Application.Features.Tariffs;


public class QueryTariffById
{
    public record Query(int Id, Expression<Func<Tariff, object>>[]? Includes = null) : IRequest<OperationResult<TariffDto>>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<TariffDto>>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult<TariffDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var includes = request.Includes;

                var items = await _repository.FindWithIncludesAsync(t => t.Id == request.Id, includes);
                var entity = items.FirstOrDefault();

                if (entity == null)
                    return OperationResult<TariffDto>.Failure("Tarifa não encontrada.");

                return OperationResult<TariffDto>.Success(entity.ToDto());
            }, "Consulta detalhada de tarifa");
        }
    }
}
