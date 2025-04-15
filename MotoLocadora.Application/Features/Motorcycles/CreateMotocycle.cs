using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Application.Features.Motorcycles.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Events;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;


public class CreateMotorcycle
{
    public record Command(MotorcycleDto Motorcycle) : IRequest<OperationResult<int>>;

    public class Handler(
        IMotorcycleRepository repository,
        IEventBusPublisher eventBusPublisher,
        ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Motorcycle.ToEntity();
                await repository.AddAsync(entity);

                if (entity.Ano == 2024)
                {
                    var @event = new MotorcycleCreatedEvent(
                        MotorcycleId: entity.Id,
                        Model: entity.Modelo,
                        LicensePlate: entity.Placa,
                        Year: entity.Ano
                    );

                    await eventBusPublisher.PublishAsync(
                        exchange: "motorcycle-exchange",
                        routingKey: "motorcycle.created",
                        @event
                    );

                    logger.LogInformation("Evento MotorcycleCreatedEvent publicado para a moto {@MotorcycleId}", entity.Id);
                }

                return OperationResult<int>.Success(entity.Id);
            }, "Criar motocicleta");
        }
    }
}
