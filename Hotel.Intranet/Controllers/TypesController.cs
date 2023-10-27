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
    public class TypesController : Controller
    {
        private readonly HotelContext _context;

        public TypesController(HotelContext context)
        {
            _context = context;
        }

        // GET: Types
        public async Task<IActionResult> Index()
        {
            var types = await _context.Types.Include(t => t.Rooms).Include(r => r.Facilities).ToListAsync();

            return View(types);
        }

        // GET: Types/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                 .Include(t => t.Rooms)
                 .Include(r => r.Facilities)
                 .FirstOrDefaultAsync(m => m.IdType == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // GET: Types/Create
        public IActionResult Create()
        {
            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");
            ViewData["Rooms"] = new SelectList(_context.Room, "IdRoom", "Number");
            return View();
        }

        // POST: Types/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdType,Name,Capacity,Description,PhotosURL,Size,MaxAmountOfPeople,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Types types, List<int> facilities)
        {
            //if (ModelState.IsValid)
            //{
            if (facilities != null)
            {
                foreach (var facilityId in facilities)
                {
                    var facility = await _context.Facilities.FindAsync(facilityId);
                    if (facility != null)
                    {
                        types.Facilities.Add(facility);
                    }
                }
                types.AddedDate = DateTime.Now;
                types.AddedBy = "Admin";
                _context.Add(types);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Facilities"] = new SelectList(_context.Facilities, "IdFacility", "NameFacility");
            return View(types);
        }

        // GET: Types/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types.FindAsync(id);
            if (types == null)
            {
                return NotFound();
            }
            return View(types);
        }

        // POST: Types/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdType,Name,Capacity,Description,PhotosURL,Size,MaxAmountOfPeople,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Types types)
        {
            if (id != types.IdType)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(types);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypesExists(types.IdType))
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
            return View(types);
        }

        // GET: Types/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Types == null)
            {
                return NotFound();
            }

            var types = await _context.Types
                .FirstOrDefaultAsync(m => m.IdType == id);
            if (types == null)
            {
                return NotFound();
            }

            return View(types);
        }

        // POST: Types/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Types == null)
            {
                return Problem("Entity set 'HotelContext.Types'  is null.");
            }
            var types = await _context.Types.FindAsync(id);
            if (types != null)
            {
                _context.Types.Remove(types);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TypesExists(int id)
        {
          return (_context.Types?.Any(e => e.IdType == id)).GetValueOrDefault();
        }
    }
}
