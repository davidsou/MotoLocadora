using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Images;
using MotoLocadora.Application.Features.Images.Dto;

namespace MotoLocadora.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
public class ImagemController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Upload([FromForm] UploadImageDto dto)
    {
        var result = await _mediator.Send(new UploadImage.Command(dto));
        return FromResult(result);
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> Get(string fileName)
    {
        var result = await _mediator.Send(new GetImage.Query(fileName));
        return result.IsSuccess && result.Value is not null
            ? File(result.Value, "application/octet-stream", fileName)
            : FromResult(result);
    }

    [HttpDelete("{fileName}")]
    public async Task<IActionResult> Delete(string fileName)
    {
        var result = await _mediator.Send(new DeleteImage.Command(fileName));
        return FromResult(result);
    }
}





