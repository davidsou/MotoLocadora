using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using System.Linq.Expressions;

namespace MotoLocadora.Application.Features.Rents;

public class QueryRentById
{
    public record Query(int Id, Expression<Func<Rent, object>>[]? Includes = null) : IRequest<OperationResult<RentDto>>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<RentDto>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<RentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var includes = request.Includes;

                var items = await _repository.FindWithIncludesAsync(r => r.Id == request.Id, includes);
                var entity = items.FirstOrDefault();

                if (entity == null)
                    return OperationResult<RentDto>.Failure("Aluguel não encontrado.");

                return OperationResult<RentDto>.Success(entity.ToDto());
            }, "Consulta detalhada de aluguel");
        }
    }
}

