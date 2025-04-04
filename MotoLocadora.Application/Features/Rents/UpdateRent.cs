using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;


public class UpdateRent
{
    public record Command(int Id, RentDto Rent) : IRequest<OperationResult>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Aluguel não encontrado.");

                entity.TariffId = request.Rent.TariffId;
                entity.RiderId = request.Rent.RiderId;
                entity.MotorcycleId = request.Rent.MotorcycleId;
                entity.Start = request.Rent.Start;
                entity.EstimateEnd = request.Rent.EstimateEnd;
                entity.End = request.Rent.End;
                entity.AppliedFine = request.Rent.AppliedFine;
                entity.FineReason = request.Rent.FineReason;
                entity.FinalPrice = request.Rent.FinalPrice;
                entity.ChangedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(entity);
                return OperationResult.Success();
            }, "Atualizar aluguel");
        }
    }
}