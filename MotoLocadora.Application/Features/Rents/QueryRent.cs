using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;

public class QueryRents
{
    public record Query(RentQueryParams Params) : IRequest<OperationResult<PagedResult<RentDto>>>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<PagedResult<RentDto>>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<PagedResult<RentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var parameters = request.Params;

                var queryOptions = new QueryOptions<Rent>
                {
                    Filter = r =>
                        (!parameters.FilterByRiderId.HasValue || r.RiderId == parameters.FilterByRiderId.Value) &&
                        (!parameters.FilterByMotorcycleId.HasValue || r.MotorcycleId == parameters.FilterByMotorcycleId.Value) &&
                        (!parameters.FilterByTariffId.HasValue || r.TariffId == parameters.FilterByTariffId.Value),
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

                return OperationResult<PagedResult<RentDto>>.Success(new PagedResult<RentDto>
                {
                    TotalCount = totalItems,
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de aluguéis");
        }
    }
}

