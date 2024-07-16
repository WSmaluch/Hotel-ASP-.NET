using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Gallery;
using Hotel.Data.Data.CMS.Attractions;

namespace Hotel.Intranet.Controllers
{
    public class GalleryController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        public GalleryController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Gallery
        public async Task<IActionResult> Index()
        {
              return _context.Gallery != null ? 
                          View(await _context.Gallery.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Gallery'  is null.");
        }

        // GET: Gallery/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Gallery == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Gallery gallery, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Zapisanie linku do obrazu w obiekcie Types
                gallery.ImageUrl = imageUrl;
            }
            else
            {
                gallery.ImageUrl = "No photo";
            }

            gallery.AddedBy = "Admin";
            gallery.AddedDate = DateTime.Now;
            gallery.IsActive = true;
                _context.Add(gallery);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Gallery/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Gallery == null)
            {
                return NotFound();
            }

            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery == null)
            {
                return NotFound();
            }
            return View(gallery);
        }

        // POST: Gallery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Gallery gallery, IFormFile photoFile)
        {
            if (id != gallery.Id)
            {
                return NotFound();
            }

            var existingOption = await _context.Gallery.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);


            if (photoFile != null && photoFile.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Zapisanie linku do obrazu w obiekcie Types
                gallery.ImageUrl = imageUrl;
            }
            else
            {
                // Zachowanie istniejącego PhotoUrl, jeśli nie przesłano nowego pliku - jest ten sam
                gallery.ImageUrl = existingOption.ImageUrl;
            }

                try
                {
                    _context.Update(gallery);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Id))
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

        // GET: Gallery/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Gallery == null)
            {
                return Problem("Entity set 'HotelContext.Gallery'  is null.");
            }
            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery != null)
            {
                _context.Gallery.Remove(gallery);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Gallery == null)
            {
                return Problem("Entity set 'HotelContext.Gallery'  is null.");
            }
            var gallery = await _context.Gallery.FindAsync(id);
            if (gallery != null)
            {
                _context.Gallery.Remove(gallery);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GalleryExists(int id)
        {
          return (_context.Gallery?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
