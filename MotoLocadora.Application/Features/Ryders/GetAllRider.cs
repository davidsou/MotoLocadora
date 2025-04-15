using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;

public class GetAllRiders
{
    public record Query() : IRequest<OperationResult<List<RiderDto>>>;

    public class Handler(IRiderRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<List<RiderDto>>>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult<List<RiderDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var list = await _repository.GetAllAsync();
                var dtos = list.Select(x => x.ToDto()).ToList();
                return OperationResult<List<RiderDto>>.Success(dtos);
            }, "Listar entregadores");
        }
    }
}