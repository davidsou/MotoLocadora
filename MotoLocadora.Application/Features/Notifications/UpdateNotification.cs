using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Notifications;

public class UpdateNotification
{
    public record Command(int Id, NotificationDto Notification) : IRequest<OperationResult>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Notificação não encontrada.");

                entity.Type = request.Notification.Type;
                entity.Description = request.Notification.Description;
                entity.ChangedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(entity);
                return OperationResult.Success();
            }, "Atualizar notificação");
        }
    }
}

// ============
