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
    public class SoftwaresController : Controller
    {
        private readonly EAdministrationContext _context;

        public SoftwaresController(EAdministrationContext context)
        {
            _context = context;
        }

        // GET: Softwares
        public async Task<IActionResult> Index()
        {
            return View(await _context.Softwares.ToListAsync());
        }

        // GET: Softwares/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .FirstOrDefaultAsync(m => m.SoftId == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // GET: Softwares/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Softwares/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SoftId,SoftwareName")] Software software)
        {
            if (ModelState.IsValid)
            {
                _context.Add(software);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(software);
        }

        // GET: Softwares/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares.FindAsync(id);
            if (software == null)
            {
                return NotFound();
            }
            return View(software);
        }

        // POST: Softwares/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SoftId,SoftwareName")] Software software)
        {
            if (id != software.SoftId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(software);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SoftwareExists(software.SoftId))
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
            return View(software);
        }

        // GET: Softwares/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var software = await _context.Softwares
                .FirstOrDefaultAsync(m => m.SoftId == id);
            if (software == null)
            {
                return NotFound();
            }

            return View(software);
        }

        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var software = await _context.Softwares.FindAsync(id);
            if (software != null)
            {
                _context.Softwares.Remove(software);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SoftwareExists(int id)
        {
            return _context.Softwares.Any(e => e.SoftId == id);
        }
    }
}
