using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Notifications;

public class QueryNotifications
{
    public record Query(NotificationQueryParams Params) : IRequest<OperationResult<PagedResult<NotificationDto>>>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<PagedResult<NotificationDto>>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult<PagedResult<NotificationDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var parameters = request.Params;

                var queryOptions = new QueryOptions<Notification>
                {
                    Filter = n =>
                        (string.IsNullOrEmpty(parameters.FilterByType) || n.Type.Contains(parameters.FilterByType)) &&
                        (string.IsNullOrEmpty(parameters.FilterByDescription) || n.Description.Contains(parameters.FilterByDescription)),
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

                return OperationResult<PagedResult<NotificationDto>>.Success(new PagedResult<NotificationDto>
                {
                    TotalCount = totalItems,
                    PageSize = pageSize,
                    CurrentPage = currentPage,
                    TotalPages = totalPages,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de notificações");
        }
    }
}
