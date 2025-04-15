using MediatR;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;

namespace MotoLocadora.Application.Features.Images;


public static class DeleteImage
{
    public record Command(string FileName) : IRequest<OperationResult>;

    public class Handler(IStorageService storageService) : IRequestHandler<Command, OperationResult>
    {
        public async Task<OperationResult> Handle(Command request, CancellationToken cancellationToken)
        {
            await storageService.DeleteAsync(request.FileName);
            return OperationResult.Success("Imagem excluída com sucesso.");
        }
    }
}
