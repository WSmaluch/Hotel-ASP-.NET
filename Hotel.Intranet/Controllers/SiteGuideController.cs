using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Site_Guid;
using Hotel.Data.Data.CMS.Attractions;

namespace Hotel.Intranet.Controllers
{
    public class SiteGuideController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        public SiteGuideController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        // GET: SiteGuide
        public async Task<IActionResult> Index()
        {
              return _context.SiteGuide != null ? 
                          View(await _context.SiteGuide.ToListAsync()) :
                          Problem("Entity set 'HotelContext.SiteGuide'  is null.");
        }

        // GET: SiteGuide/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SiteGuide == null)
            {
                return NotFound();
            }

            var siteGuide = await _context.SiteGuide
                .FirstOrDefaultAsync(m => m.Id == id);
            if (siteGuide == null)
            {
                return NotFound();
            }

            return View(siteGuide);
        }

        // GET: SiteGuide/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SiteGuide/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Details,Image,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] SiteGuide siteGuide, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Zapisanie linku do obrazu w obiekcie Types
                siteGuide.Image = imageUrl;
            }
            else
            {
                siteGuide.Image = "No photo";
            }

            siteGuide.AddedBy = "Admin";
            siteGuide.AddedDate = DateTime.Now;
            siteGuide.IsActive = true;
                _context.Add(siteGuide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: SiteGuide/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SiteGuide == null)
            {
                return NotFound();
            }

            var siteGuide = await _context.SiteGuide.FindAsync(id);
            if (siteGuide == null)
            {
                return NotFound();
            }
            return View(siteGuide);
        }

        // POST: SiteGuide/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Details,Image,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] SiteGuide siteGuide, IFormFile photoFile)
        {
            if (id != siteGuide.Id)
            {
                return NotFound();
            }

            if (photoFile != null && photoFile.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Zapisanie linku do obrazu w obiekcie Types
                siteGuide.Image = imageUrl;
            }
            else
            {
                // Zachowanie istniejącego PhotoUrl, jeśli nie przesłano nowego pliku - jest ten sam
                siteGuide.Image = siteGuide.Image;
            }

                try
                {
                    _context.Update(siteGuide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SiteGuideExists(siteGuide.Id))
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

        // GET: SiteGuide/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.SiteGuide == null)
            {
                return Problem("Entity set 'HotelContext.SiteGuide'  is null.");
            }
            var siteGuide = await _context.SiteGuide.FindAsync(id);
            if (siteGuide != null)
            {
                _context.SiteGuide.Remove(siteGuide);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: SiteGuide/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SiteGuide == null)
            {
                return Problem("Entity set 'HotelContext.SiteGuide'  is null.");
            }
            var siteGuide = await _context.SiteGuide.FindAsync(id);
            if (siteGuide != null)
            {
                _context.SiteGuide.Remove(siteGuide);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SiteGuideExists(int id)
        {
          return (_context.SiteGuide?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
