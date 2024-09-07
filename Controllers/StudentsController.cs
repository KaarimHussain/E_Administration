using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Administration.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
            var username = User.Identity.Name;
            var usernameFromClaim = User.FindFirst(ClaimTypes.Name)?.Value;
            ViewBag.Username = usernameFromClaim;
            return View();
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}
