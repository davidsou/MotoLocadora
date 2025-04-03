using Microsoft.AspNetCore.Mvc;

namespace MotoLocadora.Api.Controllers;

public class MotoController : BaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
