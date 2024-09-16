using E_Administration.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Administration.Controllers
{
    public class HeadOfDepartmentController : Controller
    {
        private readonly EAdministrationContext _context;
        public HeadOfDepartmentController(EAdministrationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ViewTeacher()
        {
            var userRole = await _context.Roles.FirstOrDefaultAsync(q => q.RoleName.ToLower() == "user" || q.RoleName.ToLower() == "student");
            var users = _context.Users.Where(u => u.RoleId == userRole.RoleId);
            return View(users);
        }

        [HttpGet]
        public IActionResult ViewAssignTeacher()
        {
            // Replace 'instituteId' with the actual Institute ID you want to filter by.
            int instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);

            // Fetching only the HodCourseAssignTeachers where the Course's InstituteId matches the given instituteId
            var assignTeacher = _context.HodCourseAssignTeachers
            .Include(q => q.Course) // Include the Course to access InstituteId
            .Include(q => q.AssignByNavigation) // Include AssignByNavigation for user details
            .Include(q => q.AssignToNavigation) // Include AssignToNavigation for user details
            .Where(q => q.Course.InstituteId == instituteId) // Filter by InstituteId from Course
            .ToList();

            return View(assignTeacher);
        }


        [HttpPost]
        public async Task<IActionResult> MakeTeacher(int userId)
        {
            var existUser = await _context.Users.Where(q => q.Id == userId).FirstOrDefaultAsync();
            if(existUser != null)
            {
                var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
                var teacherRole = _context.Roles.FirstOrDefault(u => u.RoleName.ToLower() == "teacher" || u.RoleName.ToLower() == "faculty" && u.InstituteId == instituteId);
                if(teacherRole != null)
                {
                    existUser.RoleId = teacherRole.RoleId;
                    _context.Update(existUser);
                    await _context.SaveChangesAsync();
                    TempData["HODSuccess"] = "Successfully Updated User Role";
                }
                else
                {
                    TempData["HODError"] = "Unable to Find the Teacher Role";
                }
            }
            else
            {
                TempData["HODError"] = "User does not Exist!";
            }
            return RedirectToAction("ViewTeacher");
        }

        // GET: View for assigning a course to a teacher
        [HttpGet]
        public IActionResult AddTeacher()
        {
            if (!User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var hodId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            var teachers = _context.Roles
                .Include(r => r.Institute)
                .Include(r => r.Users)
                .Where(r => r.InstituteId == instituteId && r.RoleName.ToLower() == "teacher" || r.RoleName.ToLower() == "faculty")
                .SelectMany(r => r.Users)
                .Where(u => u != null)
                .ToList();
            var courses = _context.Courses.Where(i => i.InstituteId == instituteId).ToList();

            ViewData["Teachers"] = new SelectList(teachers, "Id", "Username");
            ViewData["Course"] = new SelectList(courses, "CourseId", "CourseName");
            ViewBag.hodId = hodId;
            return View();
        }

        // POST: Handle the assignment of a course to a teacher
        [HttpPost]
        public async Task<IActionResult> AddTeacher(int hodId, int teacherId, int courseId)
        {
            if (!User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }

            // Check if the course is already assigned to the selected teacher
            var courseAlreadyAssigned = _context.HodCourseAssignTeachers
                                                 .FirstOrDefault(a => a.AssignTo == teacherId && a.CourseId == courseId);
            if (courseAlreadyAssigned != null)
            {
                TempData["HODError"] = "This course is already assigned to the selected teacher.";
                return RedirectToAction("AddTeacher");
            }

            // Create the assignment
            var assignment = new HodCourseAssignTeacher
            {
                AssignBy = hodId,
                AssignTo = teacherId,
                CourseId = courseId,
                AssignAt = DateTime.Now
            };

            _context.HodCourseAssignTeachers.Add(assignment);
            await _context.SaveChangesAsync();

            TempData["HODSuccess"] = "Successfully assigned the course to the teacher!";
            return RedirectToAction("AddTeacher");
        }

        //Course work
        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            var courses = await _context.Courses.Where(i => i.InstituteId == instituteId).ToListAsync();
            return View(courses);
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses 
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,CourseDuration,CreatedBy,CreatedAt")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseDuration,CreatedBy,CreatedAt")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                _context.Courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }

        public IActionResult IndexSchedule()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ViewSchedule()
        {
            var schedule = _context.Schedules.Include(q=> q.Day).ToList();
            return View(schedule);
        }

        [HttpGet]
        public IActionResult AddSchedule()
        {
            ViewData["Days"] = new SelectList(_context.ScheduleDays.ToList(), "DayId", "DayName");
            ViewData["Labs"] = new SelectList(_context.Labs.ToList(), "LabId", "LabId");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddSchedule(Schedule schedule)
        {
            if (!ModelState.IsValid)
            {
                TempData["HODSuccess"] = "Successfully added an Schedule";
                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction("ViewSchedule");
            }
            TempData["HODError"] = "Failed to add the Schedule";
            ViewData["Days"] = new SelectList(_context.ScheduleDays.ToList(), "DayId", "DayName");
            ViewData["Labs"] = new SelectList(_context.Labs.ToList(), "LabId", "LabId");
            return View(schedule);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleDelete(int scheduleId)
        {
            var schedule = await _context.Schedules.FirstOrDefaultAsync(q => q.ScheduleId == scheduleId);
            if (schedule != null)
            {
                _context.Remove(schedule);
                await _context.SaveChangesAsync();
                TempData["HODSuccess"] = "Successfully Removed the Schedule";
            }
            else
            {
                TempData["HODError"] = "Failed to find the Schedule!";
            }
            return View("ViewSchedule");
        }

        [HttpGet]
        public async Task<IActionResult> DaySchedule()
        {
            var days = await _context.ScheduleDays.ToListAsync();
            return View(days);
        }

        [HttpGet]
        public IActionResult AddDaySchedule()
        {
            return View();
        }

        // Method to handle form submission and add a new day
        [HttpPost]
        public async Task<IActionResult> AddDaySchedule(ScheduleDay day)
        {
            if (ModelState.IsValid)
            {
                _context.ScheduleDays.Add(day);
                await _context.SaveChangesAsync();
                return RedirectToAction("DaySchedule");
            }

            return View(day);
        }


        // ============================================================
        // MY CUSTOM CONTROLLER FOR VIEW
        // ============================================================

        // Not Gonna Use Async cause it's for Babies
        // Only use it when you gonna need it because the work above is done by a random women and work from here is done by me Kaarim Hussain
        
        // ===================================   COURSES   =====================================

        // Navigate to the Main Panel where HOD can view the Courses
        [HttpGet]
        public IActionResult ViewCourses()
        {
            if (!User.Identity.IsAuthenticated && !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Fetching the ID of the Institute
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);

            var courses = _context.Courses.Where(i => i.InstituteId == instituteId).ToList();
            ViewBag.InstituteId = instituteId;
            return View(courses);
        }


        // Navigate to AddCourses Panel to Add Courses
        [HttpGet]
        public IActionResult AddCourses()
        {
            if (!User.Identity.IsAuthenticated && !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Fetching the ID of the Institute
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);

            ViewBag.InstituteId = instituteId;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> AddCourses([Bind("CourseName", "CourseDuration", "CreatedBy", "CreatedAt","InstituteId")]Course course)
        {
            if (!User.Identity.IsAuthenticated && !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            // Fetching the ID of the Institute
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            if (ModelState.IsValid)
            {
                course.CreatedBy = "HOD";
                course.CreatedAt = DateTime.Now;
                course.InstituteId = instituteId;

                await _context.AddAsync(course);
                await _context.SaveChangesAsync();
                TempData["CourseSuccess"] = "Successfully added a Course";
            }
            else
            {
                TempData["CourseError"] = "Failed to Create Course! Provided Credentials are not Valid.";
            }
            ViewBag.InstituteId = instituteId;
            return View(course);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCourse(int CourseId)
        {
            var course = await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == CourseId);
            if (course != null)
            {
                _context.Remove(course);
                await _context.SaveChangesAsync();
                TempData["CourseSuccess"] = "Successfully Removed a Course";
            }
            else
            {
                TempData["CourseError"] = "Failed to Remove an Course";
            }
            return RedirectToAction("ViewCourses");
        }

        [HttpGet]
        public IActionResult EditCourse(int CourseId)
        {
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
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
            ViewBag.InstituteId = instituteId;
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
                cour.CreatedBy = "HOD";
                _context.Update(course);
                await _context.SaveChangesAsync();
                TempData["CourseSuccess"] = "Successfully Edited a Course";
            }
            return RedirectToAction("ViewCourses");
        }

        // ===================================   COURSES END   =====================================
        // ===================================   DEPARTMENT   =====================================

        [HttpGet]
        public IActionResult ViewDepartment()
        {
            if (!User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);

            var departments = _context.Departments
            .Where(d => d.InstituteId == instituteId)
            .ToList();

            ViewBag.InstituteId = instituteId;
            return View(departments);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
        {
            if (!User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);

            var department = await _context.Departments.FirstOrDefaultAsync(d => d.DepartmentId == departmentId);
            if (department != null)
            {
                _context.Departments.Remove(department);
                await _context.SaveChangesAsync();
                TempData["DepartmentSuccess"] = "Department was added Successfully";
                return RedirectToAction("ViewDepartment");
            }
            else
            {
                TempData["DepartmentError"] = "Department not found.";
            }

            return RedirectToAction("ViewDepartment");
        }

        //GET
        [HttpGet]
        public IActionResult AddDepartment()
        {
            if (!User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
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
            if (!User.IsInRole("HOD"))
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
                    return RedirectToAction("AddDepartment");
                }

                _context.Add(department);
                await _context.SaveChangesAsync();
                TempData["DepartmentSuccess"] = "Successfully added a department!";
                return RedirectToAction("ViewDepartment");
            }

            TempData["DepartmentError"] = "Invalid department details!" + department.InstituteId;
            return RedirectToAction("ViewDepartment");
        }

        // ===================================   DEPARTMENT END   =====================================
        // ===================================   SOFTWARES   =====================================

        [HttpGet]
        public IActionResult ViewSystem()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            ViewBag.InstituteId = instituteId;
            return View();
        }

        [HttpGet]
        public IActionResult ViewSoftware()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            ViewBag.InstituteId = instituteId;
            return View();
        }

        [HttpGet]
        public IActionResult ViewPc()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            ViewBag.InstituteId = instituteId;
            return View();
        }

        [HttpGet]
        public IActionResult ViewInstalledSoftware()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("HOD"))
            {
                return RedirectToAction("Index", "Home");
            }
            var instituteId = int.Parse(User.FindFirst("InstituteId")?.Value);
            ViewBag.InstituteId = instituteId;
            return View();
        }

        // ===================================   SOFTWARES END   =====================================
    }
}
