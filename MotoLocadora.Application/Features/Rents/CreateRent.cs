using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;
public class CreateRent
{
    public record Command(RentDto Rent) : IRequest<OperationResult<int>>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Rent.ToEntity();
                await _repository.AddAsync(entity);
                return OperationResult<int>.Success(entity.Id);
            }, "Criar aluguel");
        }
    }
}