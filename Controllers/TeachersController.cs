using Microsoft.AspNetCore.Mvc;

namespace E_Administration.Controllers
{
    public class TeachersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
