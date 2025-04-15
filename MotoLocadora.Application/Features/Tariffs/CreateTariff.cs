using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;

public class CreateTariff
{
    public record Command(TariffDto Tariff) : IRequest<OperationResult<int>>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Tariff.ToEntity();
                await _repository.AddAsync(entity);
                return OperationResult<int>.Success(entity.Id);
            }, "Criar tarifa");
        }
    }
}
