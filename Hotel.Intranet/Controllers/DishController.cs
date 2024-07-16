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
    public class DishController : Controller
    {
        private readonly HotelContext _context;

        public DishController(HotelContext context)
        {
            _context = context;
        }

        // GET: Dish
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Dish.Include(d => d.Category).Include(d => d.NutritionInfo).Include(d => d.Price).Include(d => d.Ingredients).Include(d => d.Menu);


            return View(await hotelContext.ToListAsync());
        }

        // GET: Dish/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .Include(d => d.Category)
                .Include(d => d.NutritionInfo)
                .Include(d => d.Price)
                .Include(d=> d.Menu)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dish/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.RestaurantCategory, "Id", "Name");
            ViewData["PriceId"] = new SelectList(_context.DishPrice, "Id", "Price");
            ViewBag.Ingredients = new SelectList(_context.Ingredient, "Id", "Name");
            ViewBag.Menu = new SelectList(_context.Menu, "Id", "Name");

            var nutritionInfos = _context.NutritionInfo.ToList();
            var nutritionInfoSelectList = nutritionInfos.Select(n => new
            {
                Id = n.Id,
                DisplayValue = $"Cal: {n.Calories} P: {n.Protein} CH: {n.Carbohydrates} F: {n.Fat}"
            }).ToList();

            ViewBag.NutritionInfoId = new SelectList(nutritionInfoSelectList, "Id", "DisplayValue");

            return View();
        }

        // POST: Dish/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,PriceId,CategoryId,IsVegetarian,IsVegan,IsGlutenFree,NutritionInfoId,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Dish dish, List<int> Ingredients, List<int> Menu)
        {
            ViewData["CategoryId"] = new SelectList(_context.RestaurantCategory, "Id", "Name", dish.CategoryId);
            ViewData["PriceId"] = new SelectList(_context.DishPrice, "Id", "Price", dish.PriceId);
            ViewData["NutritionInfoId"] = new SelectList(_context.NutritionInfo, "Id", "Id", dish.NutritionInfoId);

            if (Ingredients != null)
            {
                {
                    foreach (var item in Ingredients)
                    {
                        var thing = await _context.Ingredient.FindAsync(item);
                        if (thing != null)
                        {
                            dish.Ingredients.Add(thing);
                        }
                    }
                }
            }

            if (Menu != null)
            {
                {
                    foreach (var item in Menu)
                    {
                        var thing = await _context.Menu.FindAsync(item);
                        if (thing != null)
                        {
                            dish.Menu.Add(thing);
                        }
                    }
                }
            }

            _context.Add(dish);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Dish/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Ingredients = new SelectList(_context.Ingredient, "Id", "Name");
            ViewBag.Menu = new SelectList(_context.Menu, "Id", "Name");
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            //var dish = await _context.Dish.FindAsync(id);
            var dish = await _context.Dish.Include(d => d.Ingredients).Include(d=>d.Ingredients).FirstOrDefaultAsync(r => r.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            var nutritionInfos = await _context.NutritionInfo.ToListAsync();
            var nutritionInfoSelectList = nutritionInfos.Select(n => new
            {
                Id = n.Id,
                DisplayValue = $"Cal: {n.Calories} P: {n.Protein} CH: {n.Carbohydrates} F: {n.Fat}"
            }).ToList();

            ViewBag.NutritionInfoId = new SelectList(nutritionInfoSelectList, "Id", "DisplayValue", dish.NutritionInfoId);
            ViewData["CategoryId"] = new SelectList(_context.RestaurantCategory, "Id", "Name", dish.CategoryId);
            ViewData["PriceId"] = new SelectList(_context.DishPrice, "Id", "Price", dish.PriceId);
            return View(dish);
        }

        // POST: Dish/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PriceId,CategoryId,IsVegetarian,IsVegan,IsGlutenFree,NutritionInfoId,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Dish dish, List<int> Ingredients, List<int> Menu)
        {
            ViewData["CategoryId"] = new SelectList(_context.RestaurantCategory, "Id", "Name", dish.CategoryId);
            ViewData["NutritionInfoId"] = new SelectList(_context.NutritionInfo, "Id", "Id", dish.NutritionInfoId);
            ViewData["PriceId"] = new SelectList(_context.DishPrice, "Id", "Price", dish.PriceId);

            if (id != dish.Id)
            {
                return NotFound();
            }

            try
    {
        var existingDish = await _context.Dish
            .Include(d => d.Ingredients)
            .Include(d => d.Menu)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (existingDish == null)
        {
            return NotFound();
        }

        // Update dish properties
        _context.Entry(existingDish).CurrentValues.SetValues(dish);

        // Clear current ingredients and menu
        existingDish.Ingredients.Clear();
        existingDish.Menu.Clear();

        // Add new ingredients
        if (Ingredients != null && Ingredients.Count > 0)
        {
            foreach (var ingredientId in Ingredients)
            {
                var ingredient = await _context.Ingredient.FindAsync(ingredientId);
                if (ingredient != null)
                {
                    existingDish.Ingredients.Add(ingredient);
                }
            }
        }

        // Add new menu items
        if (Menu != null && Menu.Count > 0)
        {
            foreach (var menuId in Menu)
            {
                var menu = await _context.Menu.FindAsync(menuId);
                if (menu != null)
                {
                    existingDish.Menu.Add(menu);
                }
            }
        }

        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!DishExists(dish.Id))
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

        

        // GET: Dish/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Dish == null)
            {
                return Problem("Entity set 'HotelContext.Dish'  is null.");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Dish/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dish == null)
            {
                return Problem("Entity set 'HotelContext.Dish'  is null.");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
            return _context.Dish.Any(e => e.Id == id);
        }
    }
}
