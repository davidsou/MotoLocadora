using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;
public class GetAllTariffs
{
    public record Query() : IRequest<OperationResult<List<TariffDto>>>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<List<TariffDto>>>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult<List<TariffDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var list = await _repository.GetAllAsync();
                var dtos = list.Select(x => x.ToDto()).ToList();
                return OperationResult<List<TariffDto>>.Success(dtos);
            }, "Listar tarifas");
        }
    }
}
