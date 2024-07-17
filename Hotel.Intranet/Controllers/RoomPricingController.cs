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
        public ActionResult Index(int? month, int? year, int? selectedRoomTypeId)
        {
            if (!month.HasValue || !year.HasValue)
            {
                var defaultDate = DateTime.Now;
                month = defaultDate.Month;
                year = defaultDate.Year;
            }

            ViewBag.CurrentMonth = month;
            ViewBag.CurrentYear = year;

            ViewBag.RoomTypes = _context.Types
                .Select(rt => new SelectListItem
                {
                    Value = rt.IdType.ToString(),
                    Text = rt.Name
                })
                .ToList();

            if (selectedRoomTypeId.HasValue)
            {
                ViewBag.SelectedRoomTypeId = selectedRoomTypeId;
            }

            var roomPricings = _context.RoomPricing
                .Where(rp => rp.ValidFrom.Month == month && rp.ValidFrom.Year == year)
                .Include(r => r.Type)
                .ToList();

            if (selectedRoomTypeId.HasValue)
            {
                roomPricings = roomPricings.Where(rp => rp.TypeId == selectedRoomTypeId).ToList();
            }

            return View(roomPricings);
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
            List<string> errorMessages = new List<string>();

            var overlappingPricings = _context.RoomPricing
                .Where(rp => rp.TypeId == roomPricing.TypeId &&
                             rp.ValidFrom.Date <= roomPricing.ValidTo.Date &&
                             rp.ValidTo.Date >= roomPricing.ValidFrom.Date)
                .ToList();

            if (overlappingPricings.Any())
            {

                foreach (var pricing in overlappingPricings)
                {
                    errorMessages.Add($"Conflicting dates: {pricing.ValidFrom:dd/MM/yyyy} - {pricing.ValidTo:dd/MM/yyyy}");

                }
                ViewBag.ErrorMessages = errorMessages;
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
            ViewData["TypeId"] = new SelectList(_context.Types, "IdType", "Name", roomPricing.TypeId);
            Console.WriteLine($"BasePriceAdult: {roomPricing.BasePriceAdult}");
            Console.WriteLine($"BasePriceChildren: {roomPricing.BasePriceChildren}");

            if (id != roomPricing.PricingId)
            {
                return NotFound();
            }

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

        // GET: RoomPricing/Delete/5
        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.RoomPricing == null)
            {
                return Problem("Entity set 'HotelContext.Reservations'  is null.");
            }
            var pricing = await _context.RoomPricing.FindAsync(id);
            if (pricing != null)
            {
                _context.RoomPricing.Remove(pricing);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoomPricing == null)
            {
                return Problem("Entity set 'HotelContext.Reservations'  is null.");
            }
            var pricing = await _context.RoomPricing.FindAsync(id);
            if (pricing != null)
            {
                _context.RoomPricing.Remove(pricing);
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
