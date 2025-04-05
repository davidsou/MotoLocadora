using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;
public class UpdateTariff
{
    public record Command(int Id, TariffDto Tariff) : IRequest<OperationResult>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Tarifa não encontrada.");

                entity.Price = request.Tariff.Price;
                entity.Days = request.Tariff.Days;
                entity.ChangedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(entity);
                return OperationResult.Success();
            }, "Atualizar tarifa");
        }
    }
}