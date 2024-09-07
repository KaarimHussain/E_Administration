using Microsoft.AspNetCore.Mvc;

namespace E_Administration.Controllers
{
    public class RoleAuthenticatorsController : Controller
    {
        public IActionResult AuthenticateRoles()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Admins");
            }
            else if (User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Admins");
            }else if (User.IsInRole("Faculty"))
            {
                // Make sure to Create a View and Controller for the Faculty Leaving it right now as it is!
                return RedirectToAction("","");
            }
            else
            {
                // By Default Role Redirection will be on Users Home View
                return RedirectToAction("Index", "Students");
            }
        }
    }
}
