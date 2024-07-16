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
    public class CulinaryEventController : Controller
    {
        private readonly HotelContext _context;

        public CulinaryEventController(HotelContext context)
        {
            _context = context;
        }

        // GET: CulinaryEvent
        public async Task<IActionResult> Index()
        {
              return _context.CulinaryEvent != null ? 
                          View(await _context.CulinaryEvent.ToListAsync()) :
                          Problem("Entity set 'HotelContext.CulinaryEvent'  is null.");
        }

        // GET: CulinaryEvent/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CulinaryEvent == null)
            {
                return NotFound();
            }

            var culinaryEvent = await _context.CulinaryEvent
                .FirstOrDefaultAsync(m => m.Id == id);
            if (culinaryEvent == null)
            {
                return NotFound();
            }

            return View(culinaryEvent);
        }

        // GET: CulinaryEvent/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CulinaryEvent/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Date,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] CulinaryEvent culinaryEvent)
        {
            culinaryEvent.AddedBy = "Admin";
            culinaryEvent.AddedDate = DateTime.Now;
            culinaryEvent.IsActive = true;
            _context.Add(culinaryEvent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: CulinaryEvent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CulinaryEvent == null)
            {
                return NotFound();
            }

            var culinaryEvent = await _context.CulinaryEvent.FindAsync(id);
            if (culinaryEvent == null)
            {
                return NotFound();
            }
            return View(culinaryEvent);
        }

        // POST: CulinaryEvent/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Date,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] CulinaryEvent culinaryEvent)
        {
            if (id != culinaryEvent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(culinaryEvent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CulinaryEventExists(culinaryEvent.Id))
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
            return View(culinaryEvent);
        }

        // GET: CulinaryEvent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.CulinaryEvent == null)
            {
                return Problem("Entity set 'HotelContext.CulinaryEvent'  is null.");
            }
            var culinaryEvent = await _context.CulinaryEvent.FindAsync(id);
            if (culinaryEvent != null)
            {
                _context.CulinaryEvent.Remove(culinaryEvent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: CulinaryEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CulinaryEvent == null)
            {
                return Problem("Entity set 'HotelContext.CulinaryEvent'  is null.");
            }
            var culinaryEvent = await _context.CulinaryEvent.FindAsync(id);
            if (culinaryEvent != null)
            {
                _context.CulinaryEvent.Remove(culinaryEvent);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CulinaryEventExists(int id)
        {
          return (_context.CulinaryEvent?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
