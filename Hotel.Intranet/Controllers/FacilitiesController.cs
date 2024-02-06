using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.Intranet.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly HotelContext _context;

        public FacilitiesController(HotelContext context)
        {
            _context = context;
        }

        // GET: Facilities
        public async Task<IActionResult> Index()
        {
              return _context.Facilities != null ? 
                          View(await _context.Facilities.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Facilities'  is null.");
        }

        // GET: Facilities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facilities = await _context.Facilities
                .FirstOrDefaultAsync(m => m.IdFacility == id);
            if (facilities == null)
            {
                return NotFound();
            }

            return View(facilities);
        }

        // GET: Facilities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facilities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdFacility,NameFacility,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Facilities facilities)
        {
            //if (ModelState.IsValid)
            //{
                facilities.AddedDate = DateTime.Now;
                facilities.AddedBy = "Admin";
                facilities.IsActive= true;
                _context.Add(facilities);
                await _context.SaveChangesAsync();
			string previousUrl = Request.Headers["Referer"].ToString();
			return Redirect(previousUrl);
			//return RedirectToAction(nameof(Index));
			//}
			return View(facilities);
        }

        // GET: Facilities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Facilities == null)
            {
                return NotFound();
            }

            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities == null)
            {
                return NotFound();
            }
            return View(facilities);
        }

        // POST: Facilities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdFacility,NameFacility,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Facilities facilities)
        {
            if (id != facilities.IdFacility)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(facilities);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilitiesExists(facilities.IdFacility))
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
            return View(facilities);
        }

        // GET: Facilities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'HotelContext.Facilties'  is null.");
            }
            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities != null)
            {
                _context.Facilities.Remove(facilities);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Facilities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Facilities == null)
            {
                return Problem("Entity set 'HotelContext.Facilities'  is null.");
            }
            var facilities = await _context.Facilities.FindAsync(id);
            if (facilities != null)
            {
                _context.Facilities.Remove(facilities);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FacilitiesExists(int id)
        {
          return (_context.Facilities?.Any(e => e.IdFacility == id)).GetValueOrDefault();
        }
    }
}
