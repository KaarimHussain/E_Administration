using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_Administration.Models;

namespace E_Administration.Controllers
{
    public class AdditionalInfoesController : Controller
    {
        private readonly EAdministrationContext _context;

        public AdditionalInfoesController(EAdministrationContext context)
        {
            _context = context;
        }

        // GET: AdditionalInfoes
        public async Task<IActionResult> Index()
        {
            var eAdministrationContext = _context.AdditionalInfos.Include(a => a.User);
            return View(await eAdministrationContext.ToListAsync());
        }

        // GET: AdditionalInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalInfo = await _context.AdditionalInfos
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (additionalInfo == null)
            {
                return NotFound();
            }

            return View(additionalInfo);
        }

        // GET: AdditionalInfoes/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: AdditionalInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Address,PhoneNumber,ProfilePicture,DateOfBirth,Gender")] AdditionalInfo additionalInfo)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(additionalInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", additionalInfo.UserId);
            return View(additionalInfo);
        }

        // GET: AdditionalInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalInfo = await _context.AdditionalInfos.FindAsync(id);
            if (additionalInfo == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", additionalInfo.UserId);
            return View(additionalInfo);
        }

        // POST: AdditionalInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Address,PhoneNumber,ProfilePicture,DateOfBirth,Gender")] AdditionalInfo additionalInfo)
        {
            if (id != additionalInfo.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    _context.Update(additionalInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdditionalInfoExists(additionalInfo.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", additionalInfo.UserId);
            return View(additionalInfo);
        }

        // GET: AdditionalInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var additionalInfo = await _context.AdditionalInfos
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (additionalInfo == null)
            {
                return NotFound();
            }

            return View(additionalInfo);
        }

        // POST: AdditionalInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var additionalInfo = await _context.AdditionalInfos.FindAsync(id);
            if (additionalInfo != null)
            {
                _context.AdditionalInfos.Remove(additionalInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdditionalInfoExists(int id)
        {
            return _context.AdditionalInfos.Any(e => e.Id == id);
        }
    }
}
