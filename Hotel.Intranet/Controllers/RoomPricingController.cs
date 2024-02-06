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
        public ActionResult Index(int? month, int? year)
        {
            if (!month.HasValue || !year.HasValue)
            {
                // Domyślne wartości dla pierwszego załadowania strony
                var defaultDate = DateTime.Now;
                month = defaultDate.Month;
                year = defaultDate.Year;
            }

            // Pobierz dane do modelu na podstawie miesiąca i roku
            var model = _context.RoomPricing
                .Where(rp => rp.ValidFrom.Month == month && rp.ValidFrom.Year == year)
                .Include(r=>r.Type)
                .ToList();

            ViewBag.CurrentMonth = month;
            ViewBag.CurrentYear = year;

            return View(model);
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

        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PricingId,TypeId,ValidFrom,ValidTo,BasePriceAdult,BasePriceChildren,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RoomPricing roomPricing)
        {
            //if (ModelState.IsValid)
            //{
                // Sprawdź, czy istnieje już cena dla tego typu pokoju na dany dzień
                if (_context.RoomPricing.Any(rp => rp.TypeId == roomPricing.TypeId && rp.ValidFrom.Date == roomPricing.ValidFrom.Date))
                {
                    ModelState.AddModelError("ValidFrom", "Cena już istnieje dla tego typu pokoju na podany dzień.");
                    ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", roomPricing.TypeId);
                    return View(roomPricing);
                }
                else
                { 
                    roomPricing.AddedBy = "Admin";
                    roomPricing.AddedDate = DateTime.Now;
                    _context.Add(roomPricing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
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

            //if (ModelState.IsValid)
            //{
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
            //}
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

        private IEnumerable<RoomPricing> GetRoomPricingData(int month, int year)
        {
            // Tutaj dodaj kod pobierający dane z bazy danych na podstawie miesiąca i roku
            // Możesz wykorzystać Entity Framework, Dapper lub inne narzędzie dostępu do danych
            Console.WriteLine("KRZAK");
            // Poniżej znajduje się przykładowy kod, który zwraca dane dla danego miesiąca i roku
            return _context.RoomPricing
                .Include(r => r.Type)
                .Where(rp => rp.ValidFrom.Month == month && rp.ValidFrom.Year == year)
                .ToList();
        }

    }
}
