using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;

public class QueryTariffs
{
    public record Query(TariffQueryParams Params) : IRequest<OperationResult<PagedResult<TariffDto>>>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<PagedResult<TariffDto>>>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult<PagedResult<TariffDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var parameters = request.Params;

                var queryOptions = new QueryOptions<Tariff>
                {
                    Filter = t =>
                        (!parameters.FilterByPrice.HasValue || t.Price == parameters.FilterByPrice.Value) &&
                        (!parameters.FilterByDays.HasValue || t.Days == parameters.FilterByDays.Value),
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

                return OperationResult<PagedResult<TariffDto>>.Success(new PagedResult<TariffDto>
                {
                    TotalCount = totalItems,
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de tarifas");
        }
    }
}


