using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Attractions;

namespace Hotel.Intranet.Controllers
{
    public class AttractionController : Controller
    {
        private readonly HotelContext _context;

        public AttractionController(HotelContext context)
        {
            _context = context;
        }

        // GET: Attraction
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Attraction.Include(a => a.AttractionPrice).Include(a => a.AttractionType);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Attraction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Attraction == null)
            {
                return NotFound();
            }

            var attraction = await _context.Attraction
                .Include(a => a.AttractionPrice)
                .Include(a => a.AttractionType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attraction == null)
            {
                return NotFound();
            }

            return View(attraction);
        }

        // GET: Attraction/Create
        public IActionResult Create()
        {
            ViewData["AttractionPriceId"] = new SelectList(_context.AttractionPrice, "Id", "Id");
            ViewData["AttractionTypeId"] = new SelectList(_context.AttractionType, "AttractionTypeId", "Name");
            return View();
        }

        // POST: Attraction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageUrl,MoreInfoUrl,AttractionTypeId,AttractionPriceId,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Attraction attraction)
        {
            ViewData["AttractionPriceId"] = new SelectList(_context.AttractionPrice, "Id", "Id", attraction.AttractionPriceId);
            ViewData["AttractionTypeId"] = new SelectList(_context.AttractionType, "AttractionTypeId", "Name", attraction.AttractionTypeId);
            attraction.AddedBy = "Admin";
            attraction.AddedDate = DateTime.Now;
            attraction.IsActive = true;

            _context.Add(attraction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Attraction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Attraction == null)
            {
                return NotFound();
            }

            var attraction = await _context.Attraction.FindAsync(id);
            if (attraction == null)
            {
                return NotFound();
            }
            ViewData["AttractionPriceId"] = new SelectList(_context.AttractionPrice, "Id", "Id", attraction.AttractionPriceId);
            ViewData["AttractionTypeId"] = new SelectList(_context.AttractionType, "AttractionTypeId", "Name", attraction.AttractionTypeId);
            return View(attraction);
        }

        // POST: Attraction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageUrl,MoreInfoUrl,AttractionTypeId,AttractionPriceId,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Attraction attraction)
        {
            if (id != attraction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attraction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionExists(attraction.Id))
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
            ViewData["AttractionPriceId"] = new SelectList(_context.AttractionPrice, "Id", "Id", attraction.AttractionPriceId);
            ViewData["AttractionTypeId"] = new SelectList(_context.AttractionType, "AttractionTypeId", "Name", attraction.AttractionTypeId);
            return View(attraction);
        }

        // GET: Attraction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Attraction == null)
            {
                return Problem("Entity set 'HotelContext.Attraction'  is null.");
            }
            var attraction = await _context.Attraction.FindAsync(id);
            if (attraction != null)
            {
                _context.Attraction.Remove(attraction);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Attraction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Attraction == null)
            {
                return Problem("Entity set 'HotelContext.Attraction'  is null.");
            }
            var attraction = await _context.Attraction.FindAsync(id);
            if (attraction != null)
            {
                _context.Attraction.Remove(attraction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractionExists(int id)
        {
          return (_context.Attraction?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
