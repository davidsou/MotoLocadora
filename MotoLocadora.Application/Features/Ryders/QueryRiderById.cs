using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using System.Linq.Expressions;

namespace MotoLocadora.Application.Features.Ryders;

public class QueryRiderById
{
    public record Query(int Id, Expression<Func<Rider, object>>[]? Includes = null) : IRequest<OperationResult<RiderDto>>;

    public class Handler(IRiderRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<RiderDto>>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult<RiderDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var includes = request.Includes;

                var items = await _repository.FindWithIncludesAsync(r => r.Id == request.Id, includes);
                var entity = items.FirstOrDefault();

                if (entity == null)
                    return OperationResult<RiderDto>.Failure("Condutor não encontrado.");

                return OperationResult<RiderDto>.Success(entity.ToDto());
            }, "Consulta detalhada de condutor");
        }
    }
}

