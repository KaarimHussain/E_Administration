using E_Administration.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Administration.Controllers
{
    public class SignupController : Controller
    {
        private readonly EAdministrationContext _context;
        public SignupController(EAdministrationContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Signup([Bind("Username", "Email", "Password")] SignupModel lg)
        {
            if (_context.Users.Any(e => e.Email == lg.Email))
            {
                TempData["SignupError"] = "Email Already Exist!";
                return RedirectToAction("Index", "Signup");
            }
            if (ModelState.IsValid)
            {
                var user = new User();
                user.RoleId = 1;
                user.Username = lg.Username;
                user.Email = lg.Email;
                user.Password = lg.Password;
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Index", "Students");
            }
            else
            {
                TempData["SignupError"] = "Provided Credentials are not Valid!";
                return View("Index");
            }
        }


        public IActionResult Teacher_signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Teacher_Signup(SignupModel lg)
        {
            if (_context.Users.Any(u => u.Email == lg.Email))
            {
                ModelState.AddModelError("Email", "Email is already exists.");
            }
            if (ModelState.IsValid)
            {
                var user = new User();
                user.RoleId = 2;
                _context.Users.Add(user);
                _context.SaveChanges();
                ViewBag.SuccessMessage = "Welcome To The Teacher Dashboard";
            }

            return View("Teacher_View");
        }

        public IActionResult Student_signup()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult Student_Signup(SignUp lg)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        lg.RoleId = 3;
        //        db.SignUps.Add(lg);
        //        db.SaveChanges();
        //        ViewBag.SuccessMessage = "Student has been added successfully";
        //    }

        //    return View("Student_View");
        //}
    }
}
