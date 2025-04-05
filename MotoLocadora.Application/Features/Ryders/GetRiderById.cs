using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;

public class GetRiderById
{
    public record Query(int Id) : IRequest<OperationResult<RiderDto>>;

    public class Handler(IRiderRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<RiderDto>>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult<RiderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult<RiderDto>.Failure("Entregador não encontrado.");

                return OperationResult<RiderDto>.Success(entity.ToDto());
            }, "Buscar entregador por ID");
        }
    }
}
