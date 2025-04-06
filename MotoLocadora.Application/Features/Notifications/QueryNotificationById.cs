using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using System.Linq.Expressions;

namespace MotoLocadora.Application.Features.Notifications;

public class QueryNotificationById
{
    public record Query(int Id, Expression<Func<Notification, object>>[]? Includes = null) : IRequest<OperationResult<NotificationDto>>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<NotificationDto>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult<NotificationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var includes = request.Includes;

                var items = await _repository.FindWithIncludesAsync(n => n.Id == request.Id, includes);
                var entity = items.FirstOrDefault();

                if (entity == null)
                    return OperationResult<NotificationDto>.Failure("Notificação não encontrada.");

                return OperationResult<NotificationDto>.Success(entity.ToDto());
            }, "Consulta detalhada de notificação");
        }
    }
}
