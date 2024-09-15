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
    public class ServerController : Controller
    {
        private readonly EAdministrationContext _context;

        public ServerController(EAdministrationContext context)
        {
            _context = context;
        }

        // GET: Server
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if(!User.IsInRole("User") || !User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            var file = await _context.Files.ToListAsync();
            return View(file);
        }
        public async Task<IActionResult> AdminServerview()
        {
            var file = await _context.Files.ToListAsync();
            return View(file);
        }

        // GET: Server/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var file = await _context.Files
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (file == null)
            {
                return NotFound();
            }

            return View(file);
        }


        // GET: Server/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Server/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FileId,FileName,FilePath,Category,UploadDate,UploadedBy")] E_Administration.Models.File file, IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                if (uploadedFile != null && uploadedFile.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await uploadedFile.CopyToAsync(memoryStream);
                        file.FileData = memoryStream.ToArray();
                        file.FileName = uploadedFile.FileName; // Save the original file name with extension
                        file.FileName = Path.GetFullPath(uploadedFile.FileName); // Save file path if needed
                    }
                }

                _context.Add(file);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(file);
        }



        // download mathod for files with correct extensions

        public async Task<IActionResult> Download(int id)
        {
            var file = await _context.Files.FindAsync(id);
            if (file == null || file.FileData == null)
            {
                return NotFound();
            }

            var contentType = "application/octet-stream";
            var fileExtension = Path.GetExtension(file.FileName)?.ToLowerInvariant();

            // Set content type based on file extension
            switch (fileExtension)
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".doc":
                case ".docx":
                    contentType = "application/msword";
                    break;
                case ".rar":
                    contentType = "application/x-rar-compressed";
                    break;
                case ".mp3":
                    contentType = "audio/mpeg";
                    break;
                case ".mp4":
                    contentType = "video/mp4";
                    break;
                case ".avi":
                    contentType = "video/x-msvideo";
                    break;
                case ".mov":
                    contentType = "video/quicktime";
                    break;
                case ".mkv":
                    contentType = "video/x-matroska";
                    break;
                // Add more cases as needed
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return File(file.FileData, contentType, file.FileName);
        }


        // GET: Server/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @file = await _context.Files.FindAsync(id);
            if (@file == null)
            {
                return NotFound();
            }
            return View(@file);
        }

        // POST: Server/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FileId,FileName,FilePath,Category,UploadDate,UploadedBy")] E_Administration.Models.File @file)
        {
            if (id != @file.FileId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@file);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(@file.FileId))
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
            return View(@file);
        }

        // GET: Server/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @file = await _context.Files
                .FirstOrDefaultAsync(m => m.FileId == id);
            if (@file == null)
            {
                return NotFound();
            }

            return View(@file);
        }

        // POST: Server/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @file = await _context.Files.FindAsync(id);
            if (@file != null)
            {
                _context.Files.Remove(@file);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FileExists(int id)
        {
            return _context.Files.Any(e => e.FileId == id);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var file = _context.Files.FirstOrDefault(q => q.FileId == id);
            return View(file);
        }
    }
}
