using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Administration.Models;
using System.Security.Claims;

namespace E_Administration.Controllers
{
    public class ComplaintsController : Controller
    {
        private readonly EAdministrationContext _context;

        public ComplaintsController(EAdministrationContext context)
        {
            _context = context;
        }

        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated || !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var eAdministrationContext = _context.Complaints.Include(c => c.User);
            return View(await eAdministrationContext.ToListAsync());
        }

        // GET: Complaints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ComplaintsId == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // GET: Complaints/Create
        public IActionResult Create()
        {
            if(!User.Identity.IsAuthenticated || !User.IsInRole("User"))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = int.Parse(user);
            return View();
        }

        // POST: Complaints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComplaintsId,ComplaintsResponse,UserId")] Complaint complaint)
        {
            if (!User.IsInRole("User"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (!ModelState.IsValid)
            {
                var newComplaint = new Complaint
                {
                    UserId = complaint.UserId,
                    ComplaintsResponse = complaint.ComplaintsResponse,
                };
                _context.Add(newComplaint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = int.Parse(user);
            return View(complaint);
        }

        // GET: Complaints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", complaint.UserId);
            return View(complaint);
        }

        // POST: Complaints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComplaintsId,ComplaintsResponse,UserId")] Complaint complaint)
        {
            if (id != complaint.ComplaintsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(complaint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComplaintExists(complaint.ComplaintsId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", complaint.UserId);
            return View(complaint);
        }

        // GET: Complaints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ComplaintsId == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        // POST: Complaints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint != null)
            {
                _context.Complaints.Remove(complaint);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComplaintExists(int id)
        {
            return _context.Complaints.Any(e => e.ComplaintsId == id);
        }
    }
}
