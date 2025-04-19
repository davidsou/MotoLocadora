using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Enums;
using MotoLocadora.Domain.Events;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;
public class CreateRent
{
    public record Command(CreateRentDto Rent) : IRequest<OperationResult<int>>;

    public class Handler(
        IRentRepository repository,
        ITariffRepository tariffRepository,
        IRiderRepository riderRepository,
        ICurrentUserService userService,
        IEventBusPublisher eventBusPublisher,
        ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var userId = userService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return OperationResult<int>.Failure(["Usuário não autenticado."]);

                var rider = await riderRepository.GetRiderByUserIdAsync(userId);
                if (rider is null)
                    return OperationResult<int>.Failure(["Rider não encontrado."]);

                if (rider.LicenseDriveType != LicenseDriveTypeEnum.A)
                    return OperationResult<int>.Failure(["Somente entregadores com CNH tipo A podem alugar uma moto."]);

                var tariff = await tariffRepository.GetByIdAsync(request.Rent.TariffId);
                if (tariff is null)
                    return OperationResult<int>.Failure(["Tarifa inválida."]);

                var today = DateTime.UtcNow.Date;
                var expectedStart = today.AddDays(1);
                if (request.Rent.Start.Date != expectedStart)
                    return OperationResult<int>.Failure(["A data de início da locação deve ser o dia seguinte ao da criação."]);

                if (request.Rent.EstimateEnd.Date != request.Rent.Start.Date.AddDays(tariff.Days))
                    return OperationResult<int>.Failure(["A data de término estimada está incorreta para o plano selecionado."]);

                var estimatedPrice = tariff.Days * tariff.Price;

                var entity = new Rent();
                entity.RiderId = rider.Id;
                entity.EstimatedPrice = estimatedPrice;
                entity.AppliedFine = 0;
                entity.FineReason = null;
                entity.FinalPrice = estimatedPrice;

                await repository.AddAsync(entity);

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