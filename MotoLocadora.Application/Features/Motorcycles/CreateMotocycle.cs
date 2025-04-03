using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Application.Features.Motorcycles.Mappers;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Motorcycles;


public class CreateMotorcycle
{
    public record Command(MotorcycleDto Motorcycle) : IRequest<OperationResult<int>>;

       public class Handler(IMotorcycleRepository repository, ILogger<CreateMotorcycle.Handler> logger) 
        : BaseHandler(logger), IRequestHandler<Command, OperationResult<int>>
    {
        public async Task<OperationResult<int>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = request.Motorcycle.ToEntity();
                await repository.AddAsync(entity);
                return OperationResult<int>.Success(entity.Id);
            }, "Criar motocicleta");
        }
    }
}
