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
    public class PcsController : Controller
    {
        private readonly EAdministrationContext _context;

        public PcsController(EAdministrationContext context)
        {
            _context = context;
        }

        // GET: Pcs
        public async Task<IActionResult> Index()
        {
            var eAdministrationContext = _context.Pcs.Include(p => p.Hard).Include(p => p.Lab).Include(p => p.Soft);
            return View(await eAdministrationContext.ToListAsync());
        }

        // GET: Pcs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pcs
                .Include(p => p.Hard)
                .Include(p => p.Lab)
                .Include(p => p.Soft)
                .FirstOrDefaultAsync(m => m.PcId == id);
            if (pc == null)
            {
                return NotFound();
            }

            return View(pc);
        }

        // GET: Pcs/Create
        public IActionResult Create()
        {
            ViewData["HardId"] = new SelectList(_context.Hardwares, "HardId", "HardwareName");
            ViewData["LabId"] = new SelectList(_context.Labs, "LabId", "LabName");
            ViewData["SoftId"] = new SelectList(_context.Softwares, "SoftId", "SoftwareName");
            return View();
        }

        // POST: Pcs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PcId,SoftId,HardId,LabId,PcName,PurchasedAt")] Pc pc)
        {
            if (!ModelState.IsValid)
            {
                _context.Add(pc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HardId"] = new SelectList(_context.Hardwares, "HardId", "HardwareName", pc.HardId);
            ViewData["LabId"] = new SelectList(_context.Labs, "LabId", "LabName", pc.LabId);
            ViewData["SoftId"] = new SelectList(_context.Softwares, "SoftId", "SoftwareName", pc.SoftId);
            return View(pc);
        }


        // GET: Pcs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pcs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }
            ViewData["HardId"] = new SelectList(_context.Hardwares, "HardId", "HardId", pc.HardId);
            ViewData["LabId"] = new SelectList(_context.Labs, "LabId", "LabId", pc.LabId);
            ViewData["SoftId"] = new SelectList(_context.Softwares, "SoftId", "SoftId", pc.SoftId);
            return View(pc);
        }

        // POST: Pcs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PcId,SoftId,HardId,LabId,PcName,PurchasedAt")] Pc pc)
        {
            if (id != pc.PcId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PcExists(pc.PcId))
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
            ViewData["HardId"] = new SelectList(_context.Hardwares, "HardId", "HardId", pc.HardId);
            ViewData["LabId"] = new SelectList(_context.Labs, "LabId", "LabId", pc.LabId);
            ViewData["SoftId"] = new SelectList(_context.Softwares, "SoftId", "SoftId", pc.SoftId);
            return View(pc);
        }

        // GET: Pcs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pc = await _context.Pcs
                .Include(p => p.Hard)
                .Include(p => p.Lab)
                .Include(p => p.Soft)
                .FirstOrDefaultAsync(m => m.PcId == id);
            if (pc == null)
            {
                return NotFound();
            }

            return View(pc);
        }

        // POST: Pcs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pc = await _context.Pcs.FindAsync(id);
            if (pc != null)
            {
                _context.Pcs.Remove(pc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PcExists(int id)
        {
            return _context.Pcs.Any(e => e.PcId == id);
        }
    }
}
