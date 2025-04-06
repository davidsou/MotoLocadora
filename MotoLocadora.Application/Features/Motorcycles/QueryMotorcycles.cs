using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Application.Features.Motorcycles.Mappers;

namespace MotoLocadora.Application.Features.Motorcycles;

public class QueryMotorcycles
{
    public record Query(MotorcycleQueryParams Params) : IRequest<OperationResult<PagedResult<MotorcycleDto>>>;

    public class Handler(IMotorcycleRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<PagedResult<MotorcycleDto>>>
    {
        private readonly IMotorcycleRepository _repository = repository;

        public async Task<OperationResult<PagedResult<MotorcycleDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var parameters = request.Params;

                var queryOptions = new QueryOptions<Domain.Entities.Motorcycle>
                {
                    Filter = m =>
                        (string.IsNullOrEmpty(parameters.FilterByModelo) || m.Modelo.Contains(parameters.FilterByModelo)) &&
                        (string.IsNullOrEmpty(parameters.FilterByAno) || m.Ano.Contains(parameters.FilterByAno)),
                    OrderBy = parameters.OrderBy,
                    OrderDescending = parameters.OrderDescending,
                    Skip = parameters.Skip,
                    Take = parameters.Take,
                    AsNoTracking = true
                };

                var items = await _repository.QueryAsync(queryOptions);
                var totalItems = items.Count();

                return OperationResult<PagedResult<MotorcycleDto>>.Success(new PagedResult<MotorcycleDto>
                {
                    TotalCount = totalItems,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de motocicletas");
        }
    }
}
