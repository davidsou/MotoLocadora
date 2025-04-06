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

                var items = await _repository.QueryAsync(queryOptions);
                var totalItems = items.Count();

                return OperationResult<PagedResult<NotificationDto>>.Success(new PagedResult<NotificationDto>
                {
                    TotalCount = totalItems,
                    Items = items.Select(x => x.ToDto()).ToList()
                });
            }, "Consulta paginada de notificações");
        }
    }
}
