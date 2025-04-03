using Microsoft.AspNetCore.Mvc;

namespace MotoLocadora.Api.Controllers;

public class AuthController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
