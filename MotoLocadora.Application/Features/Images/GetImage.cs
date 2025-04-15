using MediatR;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Interfaces;

namespace MotoLocadora.Application.Features.Images;


public static class GetImage
{
    public record Query(string FileName) : IRequest<OperationResult<Stream>>;

    public class Handler(IStorageService storageService) : IRequestHandler<Query, OperationResult<Stream>>
    {
        public async Task<OperationResult<Stream>> Handle(Query request, CancellationToken cancellationToken)
        {
            var stream = await storageService.GetAsync(request.FileName);
            return stream is null
                ? OperationResult<Stream>.Failure(["Imagem não encontrada."])
                : OperationResult<Stream>.Success(stream);
        }
    }
}
