using Microsoft.AspNetCore.Mvc;

namespace MotoLocadora.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected IActionResult FromResult<T>(OperationResult<T> result)
        {
            if (result is null)
                return NoContent();

            return result.Success
                ? (result.Data is null ? NoContent() : Ok(result.Data))
                : BadRequest(new { error = result.ErrorMessage });
        }
    }
}
