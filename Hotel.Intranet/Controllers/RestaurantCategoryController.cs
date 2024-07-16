using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class RestaurantCategoryController : Controller
    {
        private readonly HotelContext _context;

        public RestaurantCategoryController(HotelContext context)
        {
            _context = context;
        }

        // GET: RestaurantCategory
        public async Task<IActionResult> Index()
        {
              return _context.RestaurantCategory != null ? 
                          View(await _context.RestaurantCategory.ToListAsync()) :
                          Problem("Entity set 'HotelContext.RestaurantCategory'  is null.");
        }

        // GET: RestaurantCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RestaurantCategory == null)
            {
                return NotFound();
            }

            var restaurantCategory = await _context.RestaurantCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurantCategory == null)
            {
                return NotFound();
            }

            return View(restaurantCategory);
        }

        // GET: RestaurantCategory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantCategory/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantCategory restaurantCategory)
        {
            restaurantCategory.AddedBy = "Admin";
            restaurantCategory.AddedDate = DateTime.Now;
            restaurantCategory.IsActive = true;
                _context.Add(restaurantCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: RestaurantCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RestaurantCategory == null)
            {
                return NotFound();
            }

            var restaurantCategory = await _context.RestaurantCategory.FindAsync(id);
            if (restaurantCategory == null)
            {
                return NotFound();
            }
            return View(restaurantCategory);
        }

        // POST: RestaurantCategory/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantCategory restaurantCategory)
        {
            if (id != restaurantCategory.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(restaurantCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantCategoryExists(restaurantCategory.Id))
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

        // GET: RestaurantCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.RestaurantCategory == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantCategory'  is null.");
            }
            var restaurantCategory = await _context.RestaurantCategory.FindAsync(id);
            if (restaurantCategory != null)
            {
                _context.RestaurantCategory.Remove(restaurantCategory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: RestaurantCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RestaurantCategory == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantCategory'  is null.");
            }
            var restaurantCategory = await _context.RestaurantCategory.FindAsync(id);
            if (restaurantCategory != null)
            {
                _context.RestaurantCategory.Remove(restaurantCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantCategoryExists(int id)
        {
          return (_context.RestaurantCategory?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
