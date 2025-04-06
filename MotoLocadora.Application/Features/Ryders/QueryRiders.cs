using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;

public class QueryRiders
{
    public record Query(RiderQueryParams Params) : IRequest<OperationResult<PagedResult<RiderDto>>>;

    public class Handler(IRiderRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<PagedResult<RiderDto>>>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult<PagedResult<RiderDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var parameters = request.Params;

                var queryOptions = new QueryOptions<Rider>
                {
                    Filter = r =>
                        (string.IsNullOrEmpty(parameters.FilterByName) || r.Name.Contains(parameters.FilterByName)) &&
                        (string.IsNullOrEmpty(parameters.FilterByCompanyId) || r.CommpanyId.Contains(parameters.FilterByCompanyId)) &&
                        (string.IsNullOrEmpty(parameters.FilterByLicenseDrive) || r.LicenseDrive.Contains(parameters.FilterByLicenseDrive)),
                    OrderBy = parameters.OrderBy,
                    OrderDescending = parameters.OrderDescending,
                    Skip = parameters.Skip,
                    Take = parameters.Take,
                    AsNoTracking = true
                };

                var items = await _repository.QueryAsync(queryOptions);
                var totalItems = items.Count();

                return OperationResult<PagedResult<RiderDto>>.Success(new PagedResult<RiderDto>
                {
                    TotalCount = totalItems,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de condutores");
        }
    }
}
