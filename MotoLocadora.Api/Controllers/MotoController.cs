using Microsoft.AspNetCore.Mvc;

namespace MotoLocadora.Api.Controllers;

public class MotoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
