using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;

public class GetRentById
{
    public record Query(int Id) : IRequest<OperationResult<RentDto>>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<RentDto>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<RentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult<RentDto>.Failure("Aluguel não encontrado.");

                return OperationResult<RentDto>.Success(entity.ToDto());
            }, "Buscar aluguel por ID");
        }
    }
}