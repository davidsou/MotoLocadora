using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Api.Controllers;

public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
    protected IActionResult FromResult<T>(OperationResult<T> result)
    {
        if (result is null)
            return NoContent();

        return result.IsSuccess
            ? (result.Value is null ? NoContent() : Ok(result.Value))
            : BadRequest(new { error = result.Errors });
    }
    protected IActionResult FromResult(OperationResult result)
    {
        if (result is null)
            return NoContent();

        return result.IsSuccess
            ? NoContent()
            : BadRequest(new { error = result.Errors });
    }
}
