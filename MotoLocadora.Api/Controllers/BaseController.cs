using Microsoft.AspNetCore.Mvc;
using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Api.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult FromResult<T>(OperationResult<T> result)
    {
        if (result is null)
            return NoContent();

        return result.IsSuccess
            ? (result.Value is null ? NoContent() : Ok(result.Value))
            : BadRequest(new { error = result.Errors });
    }
}
