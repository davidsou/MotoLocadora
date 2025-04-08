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

                var totalItems = await _repository.CountAsync(queryOptions.Filter);
                var items = await _repository.QueryAsync(queryOptions);

                var pageSize = parameters.Take ?? totalItems;
                var currentPage = (parameters.Skip.HasValue && pageSize > 0)
                    ? (parameters.Skip.Value / pageSize) + 1
                    : 1;

                var totalPages = pageSize > 0 ? (int)Math.Ceiling(totalItems / (double)pageSize) : 1;

                return OperationResult<PagedResult<MotorcycleDto>>.Success(new PagedResult<MotorcycleDto>
                {
                    TotalCount = totalItems,
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de motocicletas");
        }
    }
}
