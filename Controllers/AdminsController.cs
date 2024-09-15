using E_Administration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

        // GET: InstituteDetails/
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


        // GET
        public IActionResult Users()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Fetch all users with their associated HODInstitute and Institute data
            var usersWithInstitutes = _context.Users
                .Include(u => u.HodInstitutes)
                    .ThenInclude(h => h.Institute)
                .ToList();

            return View(usersWithInstitutes);
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

        //GET
        public IActionResult EditUser(int userId)
        {
            var userID = _context.Users.FirstOrDefault(id => id.Id == userId);
            return View(userID);
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

        //GET
        public async Task<IActionResult> ViewComplaints(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var complaints = await _context.Complaints.ToListAsync();
            return View(complaints);
        }

        //GET
        public IActionResult ViewLabs(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewData["Labs"] = new SelectList(_context.Labs.Where(q => q.InstituteId == id), "FloorId", "FloorName");
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpGet]
        public IActionResult AddLabs(int id)
        {
            var labs = _context.Labs.Where(q => q.InstituteId == id).ToList();
            ViewBag.InstituteId = id;
            return View(labs);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> AddLabs(int instituteId, Lab lab)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                var labs = new Lab
                {
                    InstituteId = instituteId
                };
                await _context.Labs.AddAsync(labs);
                await _context.SaveChangesAsync();
                TempData["LabsSuccess"] = "Successfully Added Lab";
            }
            else
            {
                TempData["LabsError"] = "Cannot add the Labs! Provided Credentials are not Valid";
            }
            ViewBag.InstituteId = instituteId;
            //ViewData["Floors"] = new SelectList(), "FloorId", "FloorName");
            return View();
        }

        [Route("Admins/GetLabs")]
        [HttpGet]
        public IActionResult GetLabs(int floorId)
        {
            if (floorId == 0)
            {
                return Json("No Id has been provided");
            }

            // Select only the required properties (LabId and LabName)
            var labs = _context.Labs.Where(i => i.LabId == floorId).ToList();
            return Json(labs);
        }


        //GET
        [HttpGet]
        public async Task<IActionResult> ViewCourses(int id)
        {
            var courses = await _context.Courses.Where(i => i.InstituteId == id).ToListAsync();
            ViewBag.InstituteId = id;
            return View(courses);
        }

        //GET
        [HttpGet]
        public IActionResult AddCourse(int id)
        {
            ViewBag.InstituteId = id;
            return View();
        }
        //POST
        [HttpPost]
        public async Task<IActionResult> AddCourse(Course course, int id)
        {
            if (!ModelState.IsValid)
            {
                course.CreatedBy = "Admin";
                course.CreatedAt = DateTime.Now;
                course.InstituteId = id;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                TempData["CourseSuccess"] = "Successfully added the Course";
                ViewBag.InstituteId = id;
                return View();
            }
            TempData["CourseError"] = "Failed to Add the Course";
            ViewBag.InstituteId = id;
            return View();
        }
        //POST
        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int CourseId, int id)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == CourseId);
            if (course != null)
            {
                _context.Remove(course);
                await _context.SaveChangesAsync();
                TempData["CourseSuccess"] = "Successfully Removed a Course";
            }
            TempData["CourseError"] = "Failed to Remove an Course";
            return RedirectToAction("ViewCourses", new { id = id });
        }

        [HttpGet]
        public IActionResult EditCourse(int CourseId, int id)
        {
            if (CourseId <= 0)
            {
                return RedirectToAction("ViewCourses");
            }
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == CourseId);
            if (course == null)
            {
                // Handle the case where the course is not found
                return NotFound("Course not found");
            }
            ViewBag.InstituteId = id;
            return View(course);
        }


        // POST: Save the edited course
        [HttpPost]
        public async Task<IActionResult> EditCourse(Course cour)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == cour.CourseId);
            if (course != null)
            {
                course.CourseName = cour.CourseName; // update the properties
                course.CourseDuration = cour.CourseDuration;
                cour.CreatedAt = DateTime.Now;
                cour.CreatedBy = "Admin";
                _context.Update(course);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ViewCourses");
        }

        [HttpGet]
        public IActionResult ViewHod(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var hodList = _context.HodInstitutes
                                  .Include(i => i.User)
                                  .Include(i => i.Department)
                                  .Where(h => h.InstituteId == id)
                                  .ToList();
            ViewBag.InstituteId = id;
            return View(hodList);
        }

        // ======================================
        [HttpGet]
        public IActionResult ViewDepartments(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var departments = _context.Departments
            .Where(d => d.InstituteId == id)
            .ToList();

            ViewBag.InstituteId = id;
            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int instituteId, int departmentId)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                TempData["DepartmentSuccess"] = "Department was added Successfully";
                return RedirectToAction("ViewDepartments", new { id = instituteId });
            }
            else
            {
                TempData["DepartmentError"] = "Department not found.";
            }

            return RedirectToAction("ViewDepartments", new { id = instituteId });
        }

        //GET
        [HttpGet]
        public IActionResult AddDepartments(int instituteId)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var institute = _context.Institutes.FirstOrDefault(i => i.InstituteId == instituteId);
            if (institute == null)
            {
                return NotFound(); // or handle this case as appropriate
            }

            ViewBag.InstituteId = instituteId; // Pass InstituteId to the view
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> AddDepartments(Department department)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            if (!ModelState.IsValid)
            {
                // Debugging: Check if InstituteId is being passed correctly
                var institute = await _context.Institutes.FindAsync(department.InstituteId);
                if (institute == null)
                {
                    TempData["DepartmentError"] = "Invalid Institute ID!";
                    return RedirectToAction("AddDepartments", new { id = department.InstituteId });
                }

                _context.Add(department);
                await _context.SaveChangesAsync();
                TempData["DepartmentSuccess"] = "Successfully added a department!";
                return RedirectToAction("ViewDepartments", new { id = department.InstituteId });
            }

            TempData["DepartmentError"] = "Invalid department details!" + department.InstituteId;
            return RedirectToAction("ViewDepartments", new { id = department.InstituteId });
        }

        // GET
        [HttpGet]
        public IActionResult AddHOD(int instituteId)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Fetch all users with the role name "User"
            var unassignedHODs = _context.Users.Where(u => u.RoleId == 1).ToList();

            var departments = _context.Departments.Where(u => u.InstituteId == instituteId).ToList();
            var selectedDepartment = new SelectList(departments, "DepartmentId", "DepartmentName");
            var hodUserSelectList = new SelectList(unassignedHODs, "Id", "Username");
            ViewBag.Department = selectedDepartment;
            ViewBag.UnassignedHOD = hodUserSelectList;
            ViewBag.InstituteId = instituteId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddHOD(AddHODInstituteModel hodModel, int instituteId)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Log the instituteId to verify its value
            Console.WriteLine($"Institute ID: {instituteId}");

            // Checking Model State
            if (ModelState.IsValid)
            {
                // Check if the instituteId exists in the Institute table
                var instituteExists = _context.Institutes.Any(i => i.InstituteId == instituteId);
                var userExists = _context.Users.Any(i => i.Id == hodModel.UserId);

                if (!instituteExists)
                {
                    TempData["HODError"] = "The selected institute does not exist!";
                    return RedirectToAction("AddHOD", new { instituteId = instituteId });
                }

                if (!userExists)
                {
                    TempData["HODError"] = "The selected user does not exist!";
                    return RedirectToAction("AddHOD", new { instituteId = instituteId });
                }

                // Check if the department is already assigned an HOD
                var hodAlreadyAssigned = _context.HodInstitutes.Any(h => h.DepartmentId == hodModel.DepartmentId && h.InstituteId == instituteId);
                if (hodAlreadyAssigned)
                {
                    TempData["HODError"] = "This department is already assigned to another HOD!";
                    return RedirectToAction("AddHOD", new { instituteId = instituteId });
                }

                // Create new HodInstitute entry
                var HODInst = new HodInstitute
                {
                    InstituteId = instituteId,
                    DepartmentId = hodModel.DepartmentId,
                    UserId = hodModel.UserId,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now)
                };

                // Call the ChangeRoleToHod method to update the user's role to HOD
                var roleUpdated = await ChangeRoleToHod(hodModel.UserId);

                if (roleUpdated)
                {
                    await _context.AddAsync(HODInst);
                    await _context.SaveChangesAsync();
                    TempData["HODSuccess"] = "Successfully added an HOD and assigned to a department!";
                }
                else
                {
                    TempData["HODError"] = "Failed to update user's role to HOD!";
                }

                return RedirectToAction("AddHOD", new { instituteId = instituteId });
            }

            TempData["HODError"] = "Failed to add HOD due to invalid data!";
            return RedirectToAction("AddHOD", new { instituteId = instituteId });
        }

        public async Task<bool> ChangeRoleToHod(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var hodRole = _context.Roles.FirstOrDefault(r => r.RoleName == "HOD");

            if (user != null && hodRole != null)
            {
                user.RoleId = hodRole.RoleId;
                _context.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        // =================================
        // Institute Roles
        // =================================
        public IActionResult ViewRoles(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var roles = _context.Roles.Where(u => u.InstituteId == id).ToList();
            ViewBag.InstituteId = id;
            return View(roles);
        }
        //GET
        public IActionResult AddRoles(int instituteId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InstituteId = instituteId;
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> AddRoles(string roleName, int instituteId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName.ToLower() == roleName.ToLower());
            // Create a new role since it doesn't exist
            var newRole = new Role
            {
                RoleName = roleName,
                InstituteId = instituteId
            };
            _context.Roles.Add(newRole);
            await _context.SaveChangesAsync(); // Use the async version of SaveChanges

            TempData["RoleSuccess"] = "Successfully added Role";
            ViewBag.InstituteId = instituteId;
            return View();
        }

        //POST
        [HttpPost]
        public IActionResult EditRole(int roleID, int instituteId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = _context.Roles.FirstOrDefault(id => id.RoleId == roleID);
            if (role != null)
            {
                return View(role); // Pass a single role object to the view
            }
            else
            {
                return RedirectToAction("ViewRoles", new { id = instituteId });
            }
        }

        [HttpPost]
        public IActionResult DeleteRole(int roleId, int instituteId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var role = _context.Roles.FirstOrDefault(id => id.RoleId == roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
                _context.SaveChanges();
            }
            return RedirectToAction("ViewRoles", new { id = instituteId });
        }
        // =====================================================
        // Systems
        // =====================================================
        [HttpGet]
        public IActionResult ViewSystem(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpGet]
        public IActionResult ViewHardware(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var hardware = _context.Hardwares
                .Where(u => u.InstituteId == id)
                .ToList();
            ViewBag.InstituteId = id;
            return View(hardware);
        }

        [HttpGet]
        public IActionResult ViewPcs(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            var pcs = _context.Pcs.Include(q => q.Hard).Where(u => u.InstituteId == id).ToList(); // Fetch Pc objects, not the Hardware
            ViewBag.InstituteId = id;
            return View(pcs); // Pass the list of Pc objects to the view
        }


        [HttpGet]
        public IActionResult AddPcs(int id)
        {
            var labs = _context.Labs.Where(q => q.InstituteId == id);
            var hardware = _context.Hardwares.Where(q => q.InstituteId == id);
            // Create a SelectList of the labs
            ViewData["availableLabs"] = new SelectList(_context.Labs.Where(q => q.InstituteId == id), "LabId", "LabId");
            ViewData["availableHardware"] = new SelectList(hardware, "HardId", "HardwareName");
            ViewBag.InstituteId = id;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddPcs(int id, Pc pc)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                var newPc = new Pc
                {
                    HardId = pc.HardId,
                    LabId = pc.LabId,
                    PcName = pc.PcName,
                    PurchasedAt = DateTime.Now,
                    InstituteId = id,
                };
                await _context.AddAsync(newPc);
                await _context.SaveChangesAsync();
                TempData["PcSuccess"] = "Successfully Added a PC in the Selected Lab";
            }
            else
            {
                TempData["PcError"] = "Failed to Add PC in the Selected Lab! Provided Credentials";
            }
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpGet]
        public IActionResult AddHardware(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddHardware(int Instituteid, Hardware hardware)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                var hard = new Hardware
                {
                    HardwareName = hardware.HardwareName,
                    Processor = hardware.Processor,
                    Ram = hardware.Ram,
                    OsName = hardware.OsName,
                    StorageCapacity = hardware.StorageCapacity,
                    InstituteId = Instituteid // Set the InstituteId here
                };
                await _context.AddAsync(hard);
                await _context.SaveChangesAsync();
                TempData["HardSuccess"] = "The Hardware has been added successfully";
                return RedirectToAction("ViewHardware", new { id = Instituteid });
            }
            TempData["HardError"] = "Failed to add the Hardware! The Provided credentials are not valid";
            return RedirectToAction("ViewHardware", new { id = Instituteid });
        }
        [HttpGet]
        public async Task<IActionResult> EditHardware(int id, int hardid)
        {
            var hardware = await _context.Hardwares.FirstOrDefaultAsync(u => u.HardId == hardid);

            if (hardware == null)
            {
                return NotFound();
            }

            ViewBag.InstituteId = id;
            return View(hardware);
        }

        [HttpPost]
        public async Task<IActionResult> EditHardware(Hardware model, int Instituteid)
        {
            var hardware = await _context.Hardwares.FirstOrDefaultAsync(u => u.HardId == model.HardId);

            if (hardware == null)
            {
                return NotFound();
            }

            // Update hardware properties
            hardware.HardwareName = model.HardwareName;
            hardware.Processor = model.Processor;
            hardware.Ram = model.Ram;
            hardware.OsName = model.OsName;
            hardware.StorageCapacity = model.StorageCapacity;

            await _context.SaveChangesAsync();

            TempData["HardSuccess"] = "Hardware updated successfully";
            return RedirectToAction("ViewHardware", new { id = Instituteid });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteHardware(int id, int hardid)
        {
            var hardware = await _context.Hardwares.FirstOrDefaultAsync(u => u.HardId == hardid);
            if (hardware != null)
            {
                _context.Remove(hardware);
                await _context.SaveChangesAsync();
                TempData["HardSuccess"] = "Successfully Removed the Hardware Component";
                return RedirectToAction("ViewHardware", new { id = id });
            }
            TempData["HardError"] = "Failed to Remove Hardware Component";
            return RedirectToAction("ViewHardware", new { id = id });
        }

        [HttpGet]
        public IActionResult ViewSoftware(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var software = _context.Softwares
                .Where(pc => pc.InstituteId == id)
                .ToList();
            ViewBag.InstituteId = id;
            return View(software);
        }

        [HttpGet]
        public IActionResult AddSoftware(int id)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSoftware(int id, Software soft)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var software = new Software();
            software.SoftwareName = soft.SoftwareName;
            software.PurchasedDate = soft.PurchasedDate;
            software.ExpireDate = soft.ExpireDate;
            software.InstituteId = soft.InstituteId;
            await _context.Softwares.AddAsync(software);
            await _context.SaveChangesAsync();
            TempData["SoftSuccess"] = "Successfully Added Software";
            ViewBag.InstituteId = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSoftware(int SoftwareId, int instituteId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var software = await _context.Softwares.FirstOrDefaultAsync(u => u.SoftId == SoftwareId);
            if (software != null)
            {
                _context.Softwares.Remove(software);
                await _context.SaveChangesAsync();
                TempData["SoftSuccess"] = "Successfully Deleted a Software";
            }
            else
            {
                TempData["SoftError"] = "Failed to Delete Software! Because unable to Find the Software Id";
            }

            return RedirectToAction("ViewSoftware", new { id = instituteId });
        }

        [HttpGet]
        public IActionResult EditSoftware(int id, int SoftId)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var software = _context.Softwares.FirstOrDefault(u => u.SoftId == SoftId);
            if (software != null)
            {
                ViewBag.InstituteId = id;
                return View(software);
            }
            else
            {
                TempData["SoftError"] = "Failed to Edit Software! Because unable to Find the Software Id";
                return RedirectToAction("ViewSoftware", new { id = id });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditSoftware(int InstituteId, Software software)
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            Console.WriteLine(software);
            var existingSoftware = await _context.Softwares.FirstOrDefaultAsync(u => u.SoftId == software.SoftId);
            if (existingSoftware == null)
            {
                TempData["SoftError"] = "Failed to Find the Software Data";
                return RedirectToAction("ViewSoftware", new { id = InstituteId });
            }

            existingSoftware.SoftwareName = software.SoftwareName;
            existingSoftware.PurchasedDate = software.PurchasedDate;
            existingSoftware.ExpireDate = software.ExpireDate;
            existingSoftware.InstituteId = software.InstituteId;

            _context.Softwares.Update(existingSoftware);
            await _context.SaveChangesAsync();

            TempData["SoftSuccess"] = "Successfully Updated Software Information";
            return RedirectToAction("ViewSoftware", new { id = InstituteId });
        }

        // ================================================================
        // View Teachers (Note: In Admin Panel the Teachers are only gonna be viewed because it's institute responsibility to add them view, change and remove them)
        // ================================================================

        [HttpGet]
        public async Task<IActionResult> ViewTeachers(int id)
        {
            // Get the teacher role for the specified institute
            var getTeacherRole = await _context.Roles
                .Where(u => u.InstituteId == id && u.RoleName.ToLower() == "teacher" || u.RoleName.ToLower() == "professor" || u.RoleName.ToLower() == "faculty" || u.RoleName.ToLower() == "coordinator" || u.RoleName.ToLower() == "Tutor")
                .FirstOrDefaultAsync();

            if (getTeacherRole == null)
            {
                TempData["DetailInstituteError"] = "There is no Role of Teachers in this institute";
                return RedirectToAction("DetailInstitutes", new { id = id });
            }

            // Fetch users with the retrieved role and include the role information
            var teachers = await _context.Users
                .Where(u => u.RoleId == getTeacherRole.RoleId)
                .Include(u => u.Role)
                .ToListAsync();

            ViewBag.InstituteId = id;
            return View(teachers);
        }

    }
}
