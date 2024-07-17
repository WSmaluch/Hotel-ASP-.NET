using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class PromotionController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;

        public PromotionController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Promotion
        public async Task<IActionResult> Index()
        {
              return _context.Promotion != null ? 
                          View(await _context.Promotion.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Promotion'  is null.");
        }

        // GET: Promotion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Promotion == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion
                .FirstOrDefaultAsync(m => m.Id == id);
            if (promotion == null)
            {
                return NotFound();
            }

            return View(promotion);
        }

        // GET: Promotion/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Promotion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,StartDate,EndDate,ImageUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Promotion promotion, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                // Processing the uploaded file
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Saving a link to string
                promotion.ImageUrl = imageUrl;
            }
            else
            {
                promotion.ImageUrl = "No photo";
            }

            promotion.AddedBy = "Admin";
            promotion.AddedDate = DateTime.Now;
            promotion.IsActive = true;
            _context.Add(promotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Promotion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Promotion == null)
            {
                return NotFound();
            }

            var promotion = await _context.Promotion.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }
            return View(promotion);
        }

        // POST: Promotion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,StartDate,EndDate,ImageUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Promotion promotion, IFormFile photoFile)
        {
            if (id != promotion.Id)
            {
                return NotFound();
            }

            var existingOption = await _context.Promotion.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (existingOption == null)
            {
                return NotFound();
            }

            if (photoFile != null && photoFile.Length > 0)
            {
                // Processing the uploaded file
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Saving a link to string
                promotion.ImageUrl = imageUrl;
            }
            else
            {
                // Retention of existing PhotoUrl if no new file uploaded - it is the same
                promotion.ImageUrl = existingOption.ImageUrl;
            }


            try
                {
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
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

        // GET: Promotion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Promotion == null)
            {
                return Problem("Entity set 'HotelContext.Promotion'  is null.");
            }
            var promotion = await _context.Promotion.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotion.Remove(promotion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Promotion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Promotion == null)
            {
                return Problem("Entity set 'HotelContext.Promotion'  is null.");
            }
            var promotion = await _context.Promotion.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotion.Remove(promotion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(int id)
        {
          return (_context.Promotion?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
