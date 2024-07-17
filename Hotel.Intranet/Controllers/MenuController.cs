using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class MenuController : Controller
    {
        private readonly HotelContext _context;

        public MenuController(HotelContext context)
        {
            _context = context;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
              return _context.Menu != null ? 
                          View(await _context.Menu.Include(d=>d.Dishes).ToListAsync()) :
                          Problem("Entity set 'HotelContext.Menu'  is null.");
        }

        // GET: Menu/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Menu == null)
            {
                return NotFound();
            }

            var menu = await _context.Menu
                .FirstOrDefaultAsync(m => m.Id == id);
            if (menu == null)
            {
                return NotFound();
            }

            return View(menu);
        }

        // GET: Menu/Create
        public IActionResult Create()
        {
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");

            return View();
        }

        // POST: Menu/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Menu menu, List<int> Dishes)
        {
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");

            if (Dishes != null)
            {
                foreach (var item in Dishes)
                {
                    var thing = await _context.Dish.FindAsync(item);
                    if (thing != null)
                    {
                        menu.Dishes.Add(thing);
                    }
                }
            }

                menu.AddedBy = "Admin";
            menu.AddedDate = DateTime.Now;
            menu.IsActive = true;
            _context.Add(menu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Menu/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Menu == null)
            {
                return NotFound();
            }
            ViewBag.Dishes = new SelectList(_context.Dish, "Id", "Name");
            var menu = await _context.Menu.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }
            return View(menu);
        }

        // POST: Menu/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Menu menu, List<int> Dishes)
        {
            if (id != menu.Id)
            {
                return NotFound();
            }

            var existingMenu = await _context.Menu
                    .Include(m => m.Dishes)
                    .FirstOrDefaultAsync(m => m.Id == id);

            if (existingMenu == null)
            {
                return NotFound();
            }

            // Update menu properties
            _context.Entry(existingMenu).CurrentValues.SetValues(menu);

            // Clear current dishes
            existingMenu.Dishes.Clear();

            // Add new dishes
            if (Dishes != null && Dishes.Count > 0)
            {
                foreach (var dishId in Dishes)
                {
                    var dish = await _context.Dish.FindAsync(dishId);
                    if (dish != null)
                    {
                        existingMenu.Dishes.Add(dish);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(menu.Id))
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

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Menu == null)
            {
                return Problem("Entity set 'HotelContext.Menu'  is null.");
            }
            var menu = await _context.Menu.FindAsync(id);
            if (menu != null)
            {
                _context.Menu.Remove(menu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Menu == null)
            {
                return Problem("Entity set 'HotelContext.Menu'  is null.");
            }
            var menu = await _context.Menu.FindAsync(id);
            if (menu != null)
            {
                _context.Menu.Remove(menu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuExists(int id)
        {
            return _context.Menu.Any(e => e.Id == id);
        }
    }
}
