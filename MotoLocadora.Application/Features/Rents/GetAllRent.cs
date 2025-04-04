using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;

public class GetAllRents
{
    public record Query() : IRequest<OperationResult<List<RentDto>>>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Query, OperationResult<List<RentDto>>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<List<RentDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var list = await _repository.GetAllAsync();
                var dtos = list.Select(x => x.ToDto()).ToList();
                return OperationResult<List<RentDto>>.Success(dtos);
            }, "Listar aluguéis");
        }
    }
}