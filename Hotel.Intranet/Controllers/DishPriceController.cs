using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class DishPriceController : Controller
    {
        private readonly HotelContext _context;

        public DishPriceController(HotelContext context)
        {
            _context = context;
        }

        // GET: DishPrice
        public async Task<IActionResult> Index()
        {
              return _context.DishPrice != null ? 
                          View(await _context.DishPrice.ToListAsync()) :
                          Problem("Entity set 'HotelContext.DishPrice'  is null.");
        }

        // GET: DishPrice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DishPrice == null)
            {
                return NotFound();
            }

            var dishPrice = await _context.DishPrice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dishPrice == null)
            {
                return NotFound();
            }

            return View(dishPrice);
        }

        // GET: DishPrice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DishPrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Price,EffectiveFrom,EffectiveTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] DishPrice dishPrice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dishPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dishPrice);
        }

        // GET: DishPrice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DishPrice == null)
            {
                return NotFound();
            }

            var dishPrice = await _context.DishPrice.FindAsync(id);
            if (dishPrice == null)
            {
                return NotFound();
            }
            return View(dishPrice);
        }

        // POST: DishPrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,EffectiveFrom,EffectiveTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] DishPrice dishPrice)
        {
            if (id != dishPrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dishPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DishPriceExists(dishPrice.Id))
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
            return View(dishPrice);
        }

        // GET: DishPrice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.DishPrice == null)
            {
                return Problem("Entity set 'HotelContext.DishPrice'  is null.");
            }
            var dishPrice = await _context.DishPrice.FindAsync(id);
            if (dishPrice != null)
            {
                _context.DishPrice.Remove(dishPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: DishPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DishPrice == null)
            {
                return Problem("Entity set 'HotelContext.DishPrice'  is null.");
            }
            var dishPrice = await _context.DishPrice.FindAsync(id);
            if (dishPrice != null)
            {
                _context.DishPrice.Remove(dishPrice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishPriceExists(int id)
        {
          return (_context.DishPrice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
