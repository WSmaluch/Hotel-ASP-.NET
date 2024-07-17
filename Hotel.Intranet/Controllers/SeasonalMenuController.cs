using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class SeasonalMenuController : Controller
    {
        private readonly HotelContext _context;

        public SeasonalMenuController(HotelContext context)
        {
            _context = context;
        }

        // GET: SeasonalMenu
        public async Task<IActionResult> Index()
        {
              return _context.SeasonalMenu != null ? 
                          View(await _context.SeasonalMenu.Include(sm=>sm.Dishes).ToListAsync()) :
                          Problem("Entity set 'HotelContext.SeasonalMenu'  is null.");
        }

        // GET: SeasonalMenu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SeasonalMenu == null)
            {
                return NotFound();
            }

            var seasonalMenu = await _context.SeasonalMenu
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seasonalMenu == null)
            {
                return NotFound();
            }

            return View(seasonalMenu);
        }

        // GET: SeasonalMenu/Create
        public IActionResult Create()
        {
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");
            return View();
        }

        // POST: SeasonalMenu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] SeasonalMenu seasonalMenu, List<int> Dishes)
        {
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");

            if (Dishes != null)
            {
                {
                    foreach (var item in Dishes)
                    {
                        var thing = await _context.Dish.FindAsync(item);
                        if (thing != null)
                        {
                            seasonalMenu.Dishes.Add(thing);
                        }
                    }
                }
            }

            seasonalMenu.AddedBy = "Admin";
            seasonalMenu.AddedDate = DateTime.Today;
            seasonalMenu.IsActive  = true;
            _context.Add(seasonalMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: SeasonalMenu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");

            if (id == null || _context.SeasonalMenu == null)
            {
                return NotFound();
            }

            var seasonalMenu = await _context.SeasonalMenu.FindAsync(id);
            if (seasonalMenu == null)
            {
                return NotFound();
            }
            return View(seasonalMenu);
        }

        // POST: SeasonalMenu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] SeasonalMenu seasonalMenu, List<int> Dishes)
        {
            if (id != seasonalMenu.Id)
            {
                return NotFound();
            }

            try
            {
                var existSm = await _context.SeasonalMenu
                    .Include(sm => sm.Dishes)  // Include related dishes
                    .FirstOrDefaultAsync(sm => sm.Id == id);

                if (existSm == null)
                {
                    return NotFound();
                }

                // Update scalar properties of SeasonalMenu
                _context.Entry(existSm).CurrentValues.SetValues(seasonalMenu);

                // Clear current dishes related to the seasonal menu
                existSm.Dishes.Clear();

                // Add new dishes based on the selected IDs
                if (Dishes != null && Dishes.Any())
                {
                    foreach (var dishId in Dishes)
                    {
                        var dish = await _context.Dish.FindAsync(dishId);
                        if (dish != null)
                        {
                            existSm.Dishes.Add(dish);
                        }
                    }
                }

                _context.Update(existSm);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeasonalMenuExists(seasonalMenu.Id))
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


        // GET: SeasonalMenu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.SeasonalMenu == null)
            {
                return Problem("Entity set 'HotelContext.SeasonalMenu'  is null.");
            }
            var seasonalMenu = await _context.SeasonalMenu.FindAsync(id);
            if (seasonalMenu != null)
            {
                _context.SeasonalMenu.Remove(seasonalMenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: SeasonalMenu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SeasonalMenu == null)
            {
                return Problem("Entity set 'HotelContext.SeasonalMenu'  is null.");
            }
            var seasonalMenu = await _context.SeasonalMenu.FindAsync(id);
            if (seasonalMenu != null)
            {
                _context.SeasonalMenu.Remove(seasonalMenu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeasonalMenuExists(int id)
        {
            return _context.SeasonalMenu.Any(e => e.Id == id);
        }
    }
}
