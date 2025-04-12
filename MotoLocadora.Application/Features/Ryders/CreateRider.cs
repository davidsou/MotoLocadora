using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;
public class CreateRider
{
    public record Command(RiderDto Rider) : IRequest<OperationResult<int>>;

    public class Handler(IRiderRepository repository, ICurrentUserService userService, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var userId = userService.UserId;

                var entity = request.Rider.ToEntity(userId);                
                await _repository.AddAsync(entity);
                return OperationResult<int>.Success(entity.Id);
            }, "Criar entregador");
        }
    }
}