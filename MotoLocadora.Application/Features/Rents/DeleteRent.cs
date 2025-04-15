using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Rents;

public class DeleteRent
{
    public record Command(int Id) : IRequest<OperationResult>;

    public class Handler(IRentRepository repository, ILogger<Handler> logger)
        : BaseHandler(logger), IRequestHandler<Command, OperationResult>
    {
        private readonly IRentRepository _repository = repository;

        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {
                var entity = await _repository.GetByIdAsync(request.Id);
                if (entity == null)
                    return OperationResult.Failure("Aluguel não encontrado.");

                await _repository.DeleteAsync(entity);
                return OperationResult.Success();
            }, "Deletar aluguel");
        }
    }
}
