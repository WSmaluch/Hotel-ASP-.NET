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
    public class NutritionInfoController : Controller
    {
        private readonly HotelContext _context;

        public NutritionInfoController(HotelContext context)
        {
            _context = context;
        }

        // GET: NutritionInfo
        public async Task<IActionResult> Index()
        {
              return _context.NutritionInfo != null ? 
                          View(await _context.NutritionInfo.ToListAsync()) :
                          Problem("Entity set 'HotelContext.NutritionInfo'  is null.");
        }

        // GET: NutritionInfo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NutritionInfo == null)
            {
                return NotFound();
            }

            var nutritionInfo = await _context.NutritionInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (nutritionInfo == null)
            {
                return NotFound();
            }

            return View(nutritionInfo);
        }

        // GET: NutritionInfo/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NutritionInfo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Calories,Protein,Carbohydrates,Fat,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] NutritionInfo nutritionInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nutritionInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(nutritionInfo);
        }

        // GET: NutritionInfo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NutritionInfo == null)
            {
                return NotFound();
            }

            var nutritionInfo = await _context.NutritionInfo.FindAsync(id);
            if (nutritionInfo == null)
            {
                return NotFound();
            }
            return View(nutritionInfo);
        }

        // POST: NutritionInfo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Calories,Protein,Carbohydrates,Fat,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] NutritionInfo nutritionInfo)
        {
            if (id != nutritionInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nutritionInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NutritionInfoExists(nutritionInfo.Id))
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
            return View(nutritionInfo);
        }

        // GET: NutritionInfo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.NutritionInfo == null)
            {
                return Problem("Entity set 'HotelContext.NutritionInfo'  is null.");
            }
            var nutritionInfo = await _context.NutritionInfo.FindAsync(id);
            if (nutritionInfo != null)
            {
                _context.NutritionInfo.Remove(nutritionInfo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: NutritionInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NutritionInfo == null)
            {
                return Problem("Entity set 'HotelContext.NutritionInfo'  is null.");
            }
            var nutritionInfo = await _context.NutritionInfo.FindAsync(id);
            if (nutritionInfo != null)
            {
                _context.NutritionInfo.Remove(nutritionInfo);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NutritionInfoExists(int id)
        {
          return (_context.NutritionInfo?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
