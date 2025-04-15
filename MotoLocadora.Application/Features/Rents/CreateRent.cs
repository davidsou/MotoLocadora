using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Events;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;
public class CreateRent
{
    public record Command(RentDto Rent) : IRequest<OperationResult<int>>;

    public class Handler(IRentRepository repository,
                IEventBusPublisher eventBusPublisher,
                ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Rent.ToEntity();
                await _repository.AddAsync(entity);
               
                var @event = new RentCreatedEvent(
                    RentId: entity.Id,
                    RiderId: entity.RiderId,
                    MotorcycleId: entity.MotorcycleId,
                    StartDate: entity.Start,
                    EstimateEndDate: entity.EstimateEnd
                    );

                await eventBusPublisher.PublishAsync(
                exchange: "rent-exchange",
                routingKey: "rent.created",
                    @event
                );

                logger.LogInformation("Evento RentCreatedEvent publicado para a locação {@RentId}", entity.Id);

                return OperationResult<int>.Success(entity.Id);


            }, "Criar aluguel");
        }
    }
}