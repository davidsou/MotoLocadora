using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Notifications;

public class GetAllNotifications
{
    public record Query() : IRequest<OperationResult<List<NotificationDto>>>;

    public class Handler(INotificationRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<List<NotificationDto>>>
    {
        private readonly INotificationRepository _repository = repository;

        public async Task<OperationResult<List<NotificationDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var list = await _repository.GetAllAsync();
                var dtos = list.Select(x => x.ToDto()).ToList();
                return OperationResult<List<NotificationDto>>.Success(dtos);
            }, "Listar notificações");
        }
    }
}
