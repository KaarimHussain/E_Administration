using E_Administration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq; // Make sure to include this for LINQ operations

namespace E_Administration.Controllers
{
    public class AdminsController : Controller
    {
        private readonly EAdministrationContext _context;

        public AdminsController(EAdministrationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["Students"] = _context.Users.Count();
            ViewData["Faculty"] = _context.Users.Count(u => u.RoleId == 4);
            ViewData["HOD"] = _context.Users.Count(u => u.RoleId == 2);
            ViewData["Labs"] = _context.Labs.Count();
            ViewData["PC"] = _context.Pcs.Count();
            ViewData["Software"] = _context.Softwares.Count();

            return View();
        }
        // GET
        public IActionResult Users()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var hodRoleID = _context.Roles.FirstOrDefault(id => id.RoleName == "HOD");
            if (hodRoleID != null)
            {
                ViewBag.HodRoleID = hodRoleID;
            }
            var users = _context.Users.Where(role => role.RoleId == 1).ToList();
            return View(users);
        }

        // GET
        public IActionResult PC()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var pcs = _context.Pcs.Include(pc => pc.Lab).ToList(); // Include the related Labs entity
            return View(pcs);
        }


        [HttpPost]
        public IActionResult UpdateRoleToHOD(int userId, int roleId) 
        {
            if (!User.Identity.IsAuthenticated ||  !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index","Home");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var roleExist  = _context.Roles.FirstOrDefault(u =>u.RoleId == roleId);
            if (user != null && roleExist != null)
            {
                user.RoleId = roleId;
                _context.SaveChanges();
            }
            else
            {
                TempData["UserError"] = "User do not Exist OR Role has been removed";
            }
            return RedirectToAction("Users");
        }
        [HttpPost]
        public IActionResult DeleteUser(int userId) 
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                _context.Remove(user);
                _context.SaveChanges();
            }else
            {
                TempData["AdminError"] = "Couldn't find the User!";
            }
            return RedirectToAction("Users");
        }
        // GET
        public IActionResult AddUser()
        {
            return View();
        }
    }
}
