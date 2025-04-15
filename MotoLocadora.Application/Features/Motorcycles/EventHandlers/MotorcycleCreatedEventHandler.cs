using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Events;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles.EventHandlers;

public class MotorcycleCreatedEventHandler(
    INotificationRepository notificationRepository,
    ILogger<MotorcycleCreatedEventHandler> logger)
    : IEventHandler<MotorcycleCreatedEvent>
{
    public async Task HandleAsync(MotorcycleCreatedEvent @event)
    {
        var notification = new Notification
        {
            Type = nameof(MotorcycleCreatedEvent),
            Description = $"Moto criada: {@event.Model} - Placa {@event.LicensePlate} - Ano {@event.Year}"
        };

        await notificationRepository.AddAsync(notification);


        logger.LogInformation("Notificação registrada para moto {@MotorcycleId}", @event.MotorcycleId);
    }
}
