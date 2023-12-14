using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking;

namespace Hotel.Intranet.Controllers
{
    public class RoomPricingController : Controller
    {
        private readonly HotelContext _context;

        public RoomPricingController(HotelContext context)
        {
            _context = context;
        }

        // GET: RoomPricing
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.RoomPricing.Include(r => r.Type);
            return View(await hotelContext.ToListAsync());
        }

        // GET: RoomPricing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RoomPricing == null)
            {
                return NotFound();
            }

            var roomPricing = await _context.RoomPricing
                .Include(r => r.Type)
                .FirstOrDefaultAsync(m => m.PricingId == id);
            if (roomPricing == null)
            {
                return NotFound();
            }

            return View(roomPricing);
        }

        // GET: RoomPricing/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name");
            return View();
        }

        // POST: RoomPricing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PricingId,TypeId,ValidFrom,ValidTo,BasePriceAdult,BasePriceChildren,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RoomPricing roomPricing)
        {
            //if (ModelState.IsValid)
            //{
            roomPricing.AddedDate = DateTime.Now;
            roomPricing.AddedBy = "Admin";
            _context.Add(roomPricing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", roomPricing.TypeId);
            return View(roomPricing);
        }

        // GET: RoomPricing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RoomPricing == null)
            {
                return NotFound();
            }

            var roomPricing = await _context.RoomPricing.FindAsync(id);
            if (roomPricing == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", roomPricing.TypeId);
            return View(roomPricing);
        }

        // POST: RoomPricing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PricingId,TypeId,ValidFrom,ValidTo,BasePriceAdult,BasePriceChildren,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RoomPricing roomPricing)
        {
            if (id != roomPricing.PricingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(roomPricing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomPricingExists(roomPricing.PricingId))
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
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", roomPricing.TypeId);
            return View(roomPricing);
        }

        // GET: RoomPricing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RoomPricing == null)
            {
                return NotFound();
            }

            var roomPricing = await _context.RoomPricing
                .Include(r => r.Type)
                .FirstOrDefaultAsync(m => m.PricingId == id);
            if (roomPricing == null)
            {
                return NotFound();
            }

            return View(roomPricing);
        }

        // POST: RoomPricing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoomPricing == null)
            {
                return Problem("Entity set 'HotelContext.RoomPricing'  is null.");
            }
            var roomPricing = await _context.RoomPricing.FindAsync(id);
            if (roomPricing != null)
            {
                _context.RoomPricing.Remove(roomPricing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoomPricingExists(int id)
        {
          return (_context.RoomPricing?.Any(e => e.PricingId == id)).GetValueOrDefault();
        }
    }
}
