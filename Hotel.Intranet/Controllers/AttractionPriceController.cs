using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Attractions;

namespace Hotel.Intranet.Controllers
{
    public class AttractionPriceController : Controller
    {
        private readonly HotelContext _context;

        public AttractionPriceController(HotelContext context)
        {
            _context = context;
        }

        // GET: AttractionPrice
        public async Task<IActionResult> Index()
        {
              return _context.AttractionPrice != null ? 
                          View(await _context.AttractionPrice.ToListAsync()) :
                          Problem("Entity set 'HotelContext.AttractionPrice'  is null.");
        }

        // GET: AttractionPrice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AttractionPrice == null)
            {
                return NotFound();
            }

            var attractionPrice = await _context.AttractionPrice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attractionPrice == null)
            {
                return NotFound();
            }

            return View(attractionPrice);
        }

        // GET: AttractionPrice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttractionPrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] AttractionPrice attractionPrice)
        {
            attractionPrice.AddedBy = "Admin";
            attractionPrice.AddedDate = DateTime.Now;
            attractionPrice.IsActive = true;
            _context.Add(attractionPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            return View(attractionPrice);
        }

        // GET: AttractionPrice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AttractionPrice == null)
            {
                return NotFound();
            }

            var attractionPrice = await _context.AttractionPrice.FindAsync(id);
            if (attractionPrice == null)
            {
                return NotFound();
            }
            return View(attractionPrice);
        }

        // POST: AttractionPrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] AttractionPrice attractionPrice)
        {
            if (id != attractionPrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attractionPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionPriceExists(attractionPrice.Id))
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
            return View(attractionPrice);
        }

        // GET: AttractionPrice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.AttractionPrice == null)
            {
                return Problem("Entity set 'HotelContext.AttractionPrice'  is null.");
            }
            var attractionPrice = await _context.AttractionPrice.FindAsync(id);
            if (attractionPrice != null)
            {
                _context.AttractionPrice.Remove(attractionPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AttractionPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AttractionPrice == null)
            {
                return Problem("Entity set 'HotelContext.AttractionPrice'  is null.");
            }
            var attractionPrice = await _context.AttractionPrice.FindAsync(id);
            if (attractionPrice != null)
            {
                _context.AttractionPrice.Remove(attractionPrice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractionPriceExists(int id)
        {
          return (_context.AttractionPrice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
