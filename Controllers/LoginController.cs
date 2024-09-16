using E_Administration.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // All the Login Authentication Are Handle Here
        [HttpPost]
        public async Task<IActionResult> Index([Bind("Email", "Password")] LoginModel login)
        {
            // Admin Panel Redirection
            if (login.Email == "truckdriver@gmail.com" && login.Password == "12345")
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "Admin"),
                        new Claim(ClaimTypes.Email, "truckdriver@gmail.com"),
                        new Claim("PhoneNumber", "541561331"),
                    };

                var identity = new ClaimsIdentity(claims, "Login");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Admins");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email && u.Password == login.Password);
            // Users
            if (user != null)
            {
                var getRole = await _context.Roles.FirstOrDefaultAsync(u => u.RoleId == user.RoleId);
                if (getRole != null)
                {
                    // Checking Teacher Role if exist and Redirecting Teacher to there panel
                    if (getRole.RoleName.ToLower() == "teacher" || getRole.RoleName.ToLower() == "teachers" || getRole.RoleName.ToLower() == "faculty" || getRole.RoleName.ToLower() == "faculties")
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Username),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim("InstituteId", getRole.InstituteId.ToString()),
                                new Claim(ClaimTypes.Role, "Teacher"),
                            };

                        var identity = new ClaimsIdentity(claims, "Login");
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);
                        return RedirectToAction("Index", "Teachers");
                    }
                    else if (getRole.RoleName.ToLower() == "hod" || getRole.RoleName.ToLower() == "headofdepartment")
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Username),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim("InstituteId", getRole.InstituteId.ToString()),
                                new Claim(ClaimTypes.Role, "HOD"),
                            };

                        var identity = new ClaimsIdentity(claims, "Login");
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);
                        return RedirectToAction("Index", "HeadOfDepartment");
                    }
                    else if(getRole.RoleName.ToLower() == "tech" || getRole.RoleName.ToLower() == "tech-staff" || getRole.RoleName.ToLower() == "techstaff")
                    {
                        var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.Name, user.Username),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim("InstituteId", getRole.InstituteId.ToString()),
                                new Claim(ClaimTypes.Role, "Tech"),
                            };

                        var identity = new ClaimsIdentity(claims, "Login");
                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(principal);
                        return RedirectToAction("Index", "TechStaff");
                    }
                    else
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

                        await HttpContext.SignInAsync(principal);

                        return RedirectToAction("Index", "Students"); // Redirect to another action after successful login
                    }
                    // Checking HOD Role if Exist 
                }
                else
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

                    await HttpContext.SignInAsync(principal);

                    return RedirectToAction("Index", "Students"); // Redirect to another action after successful login
                }
            }
            else
            {
                TempData["LoginError"] = "Invalid username or password.";
                return View();
            }
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
