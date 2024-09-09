using E_Administration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        //GET
        public async Task<IActionResult> Index()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var institutes = await _context.Institutes.ToListAsync();
            if (institutes == null || !institutes.Any())
            {
                ViewBag.Message = "No Institute Found";
            }
            return View(institutes);
        }
        // GET
        public IActionResult AddInstitute()
        {
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> AddInstitute(Institute inst)
        {
            if (ModelState.IsValid)
            {
                inst.CreatedAt = DateTime.Now; // Set the CreatedAt field
                _context.Institutes.Add(inst);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // If validation fails, redisplay the form
            return View(inst);
        }

        // GET: InstituteDetails/5
        public async Task<IActionResult> DetailInstitutes(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var institute = await _context.Institutes
                .FirstOrDefaultAsync(i => i.InstituteId == id);

            if (institute == null)
            {
                return NotFound();
            }

            return View(institute);
        }

        public async Task<IActionResult> ViewFloor(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var floors = await _context.Floors
                .Where(f => f.InstituteId == id)
                .Include(f => f.Institute) // Eagerly load the Institute
                .ToListAsync(); // Ensure you get a list

            return View(floors); // Pass the list to the view
        }

        // GET
        public IActionResult AddFloor()
        {
            ViewData["InstituteId"] = new SelectList(_context.Institutes, "InstituteId", "InstituteName");
            return View();
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> AddFloor(Floor floor)
        {

            if (!ModelState.IsValid)
            {
                bool floorExists = await _context.Floors
            .AnyAsync(f => f.FloorName == floor.FloorName && f.InstituteId == floor.InstituteId);
                if (!floorExists)
                {
                    floor.CreatedAt = DateTime.Now;
                    _context.Floors.Add(floor);
                    await _context.SaveChangesAsync();
                    ViewData["InstituteId"] = new SelectList(_context.Institutes, "InstituteId", "InstituteName", floor.InstituteId);
                    TempData["FloorSuccess"] = "The Floor was added Successfully";
                    return View(); // Redirect to a relevant page after successful addition
                }
                else
                {
                    TempData["FloorError"] = $"{floor.FloorName} Already Exist in the Database";
                    return View(floor);
                }

            }

            // If model validation fails, re-display the form with validation errors
            ViewData["InstituteId"] = new SelectList(_context.Institutes, "InstituteId", "InstituteName", floor.InstituteId);
            TempData["FloorError"] = "Provided Credentials are not Valid! Please try again later";
            return View(floor);
        }

        // POST
        [HttpPost]
        public async Task<IActionResult> DeleteFloor(int floorId)
        {
            // Find the floor by its ID
            var floor = await _context.Floors.FindAsync(floorId);

            if (floor != null)
            {
                _context.Floors.Remove(floor);
                await _context.SaveChangesAsync();

                TempData["FloorSuccess"] = "The floor was successfully deleted.";
            }
            else
            {
                TempData["FloorError"] = "The floor could not be found.";
            }
            return RedirectToAction("ViewFloor");
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
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var roleExist = _context.Roles.FirstOrDefault(u => u.RoleId == roleId);
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
            }
            else
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

        //GET
        public IActionResult EditUser(int userId)
        {
            var userID = _context.Users.FirstOrDefault(id => id.Id == userId);
            return View(userID);
        }

        //GET
        public IActionResult Roles()
        {
            var roles = _context.Roles.ToList();
            return View(roles);
        }
        //GET
        public IActionResult AddRoles()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> AddRoles(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
            if (role == null)
            {
                // Create a new role since it doesn't exist
                var newRole = new Role { RoleName = roleName };
                _context.Roles.Add(newRole);
                await _context.SaveChangesAsync(); // Use the async version of SaveChanges

                TempData["RoleSuccess"] = "Successfully added Role";
                return View();
            }
            else
            {
                TempData["RoleError"] = "Failed to Add Role: Role already exists";
                return View();
            }
        }

        //POST
        [HttpPost]
        public IActionResult EditRole(int roleID)
        {
            var role = _context.Roles.FirstOrDefault(id => id.RoleId == roleID);
            if (role != null)
            {
                return View(role); // Pass a single role object to the view
            }
            else
            {
                return RedirectToAction("Roles");
            }
        }

        [HttpPost]
        public IActionResult changeRoleName(string roleName, int roleId)
        {
            var role = _context.Roles.FirstOrDefault(id => id.RoleId == roleId);
            if (role != null)
            {
                role.RoleName = roleName;  // Assign the new role name
                _context.Roles.Update(role);
                _context.SaveChanges();
            }
            return RedirectToAction("Roles"); // Redirect after saving changes
        }

        [HttpPost]
        public IActionResult deleteRole(int roleId)
        {
            var role = _context.Roles.FirstOrDefault(id => id.RoleId == roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
            return RedirectToAction("Roles");
        }

        //GET
        public async Task<IActionResult> ViewComplaints(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var complaints = _context.Complaints.ToList();
            return View(complaints);
        }

        //GET
        public IActionResult ViewLabs(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["LabsId"] = new SelectList(_context.Floors.Where(fl => fl.InstituteId == id), "FloorId", "FloorName");
            return View();
        }
        //GET AJAX LABS
        [HttpGet]
        public IActionResult GetLabs(int floorId)
        {
            var labs = _context.Labs.Where(l => l.FloorId == floorId).ToList();
            return Json(labs);
        }

        //GET AJAX PC COUNT
        [HttpGet]
        public IActionResult GetPcCount(int labId)
        {
            var pc = _context.Pcs.Where(p => p.LabId == labId);
            return Json(pc);
        }





    }
}
