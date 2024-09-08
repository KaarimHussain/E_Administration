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
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admins");
                }
                else if (User.IsInRole("User"))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }
    }
}
