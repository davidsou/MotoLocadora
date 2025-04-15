using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;
public class DeleteRider
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler(IRiderRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly IRiderRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Entregador não encontrado.");

                await _repository.DeleteAsync(entity);
                return OperationResult.Success();
            }, "Deletar entregador");
        }
    }
}