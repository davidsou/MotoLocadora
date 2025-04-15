using MediatR;
using Microsoft.Extensions.Logging;
using MotoLocadora.Application.Features.Images.Dto;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Application.Features.Images;


public class UploadImage
{
    public record Command(UploadImageDto Dto) : IRequest<OperationResult<string>>;

    public class UploadImageHandler(
        IStorageService storageService,
        ICurrentUserService currentUserService,
        IRiderRepository riderRepository,
        ILogger<UploadImageHandler> logger) : BaseHandler(logger), IRequestHandler<Command, OperationResult<string>>
    {
        public async Task<OperationResult<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            return await TryCatchAsync(async () =>
            {

                var userId = currentUserService.UserId;
                if (string.IsNullOrEmpty(userId))
                    return OperationResult<string>.Failure(["Usuário não autenticado."]);

                var rider = await riderRepository.GetRiderByUserIdAsync( userId);
                if (rider is null)
                    return OperationResult<string>.Failure(["Entregador não encontrado para o usuário atual."]);

                if (!string.IsNullOrEmpty(rider.LicenseDriveImageLink))
                    return OperationResult<string>.Failure(["Entregador já possui imagem cadastrada."]);

                var file = request.Dto.File;
                var url = await storageService.UploadAsync(file.OpenReadStream(), file.FileName, file.ContentType);

                rider.LicenseDriveImageLink = url;
                await riderRepository.UpdateAsync(rider);

                return OperationResult<string>.Success(url, "Imagem enviada e associada ao Entregador com sucesso.");
            }, "Upload da imagem do Entregador");
        }
    }
}
