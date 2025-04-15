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
            ? Ok(new { result.Value, message = result.Message })
            : BadRequest(new { error = result.Errors });
    }

    protected IActionResult FromResult(OperationResult result)
    {
        if (result is null)
            return NoContent();

        return result.IsSuccess
            ? Ok(new { message = result.Message })
            : BadRequest(new { error = result.Errors });
    }
}
