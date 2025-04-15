using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Notifications;

public class CreateNotification
{
    public record Command(NotificationDto Notification) : IRequest<OperationResult<int>>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Notification.ToEntity();
                await _repository.AddAsync(entity);
                return OperationResult<int>.Success(entity.Id);
            }, "Criar notificação");
        }
    }
}
