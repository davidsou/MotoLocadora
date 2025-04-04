using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Notifications;

public class GetNotificationById
{
    public record Query(int Id) : IRequest<OperationResult<NotificationDto>>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<NotificationDto>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult<NotificationDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult<NotificationDto>.Failure("Notificação não encontrada.");

                return OperationResult<NotificationDto>.Success(entity.ToDto());
            }, "Buscar notificação por ID");
        }
    }
}
