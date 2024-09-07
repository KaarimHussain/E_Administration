using E_Administration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Administration.Controllers
{
    public class LoginController : Controller
    {
        private readonly EAdministrationContext _context;

        public LoginController(EAdministrationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind("Email", "Password")] LoginModel login)
        {
            if (ModelState.IsValid)
            {
                if (login.Email == "truckdriver@gmail.com" && login.Password == "12345")
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                    };

                    var identity = new ClaimsIdentity(claims, "Login");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Admins");
                }

                var user = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                    var identity = new ClaimsIdentity(claims, "Login");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Students"); // Redirect to another action after successful login
                }
                else
                {
                    TempData["LoginError"] = "Invalid username or password.";
                    return View();
                }
            }
            TempData["LoginError"] = "Invalid username or password.";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
