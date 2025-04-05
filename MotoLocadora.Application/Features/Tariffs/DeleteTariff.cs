using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Tariffs;

public class DeleteTariff
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler(ITariffRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly ITariffRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Tarifa não encontrada.");

                await _repository.DeleteAsync(entity);
                return OperationResult.Success();
            }, "Deletar tarifa");
        }
    }
}
