using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Events;

namespace MotoLocadora.Application.Features.Rents.EventHandlers;

public class RentCreatedEventHandler : IEventHandler<RentCreatedEvent>
{
    public Task HandleAsync(RentCreatedEvent @event)
    {
        Console.WriteLine($"[RentCreatedEventHandler] Nova locação criada: {@event.RentId}");

        // TODO: Aqui você pode incluir lógica futura, como enviar notificação por e-mail, criar notificação etc.

        return Task.CompletedTask;
    }
}