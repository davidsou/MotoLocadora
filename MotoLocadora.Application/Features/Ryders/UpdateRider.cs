using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Ryders;
public class UpdateRider
{
    public record Command(int Id, RiderDto Rider) : IRequest<OperationResult>;

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

                entity.Name = request.Rider.Name;
                entity.CommpanyId = request.Rider.CommpanyId;
                entity.BirthDate = request.Rider.BirthDate;
                entity.LicenseDrive = request.Rider.LicenseDrive;
                entity.LicenseDriveType = request.Rider.LicenseDriveType;
                entity.LicenseDriveImageLink = request.Rider.LicenseDriveImageLink;
                entity.Type = request.Rider.Type;
                entity.Email = request.Rider.Email;
                entity.Phone = request.Rider.Phone;
                entity.ChangedAt = DateTime.UtcNow;

                await _repository.UpdateAsync(entity);
                return OperationResult.Success();
            }, "Atualizar entregador");
        }
    }
}

