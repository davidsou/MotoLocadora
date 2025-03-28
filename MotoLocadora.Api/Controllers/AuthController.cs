using Microsoft.AspNetCore.Mvc;

namespace MotoLocadora.Api.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
