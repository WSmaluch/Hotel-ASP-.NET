using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking;

namespace Hotel.Intranet.Controllers
{
    public class DiscountCodeController : Controller
    {
        private readonly HotelContext _context;

        public DiscountCodeController(HotelContext context)
        {
            _context = context;
        }

        // GET: DiscountCode
        public async Task<IActionResult> Index()
        {
              return _context.DiscountCode != null ? 
                          View(await _context.DiscountCode.ToListAsync()) :
                          Problem("Entity set 'HotelContext.DiscountCode'  is null.");
        }

        // GET: DiscountCode/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DiscountCode == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCode
                .FirstOrDefaultAsync(m => m.IdDiscountCode == id);
            if (discountCode == null)
            {
                return NotFound();
            }

            return View(discountCode);
        }

        // GET: DiscountCode/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiscountCode/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDiscountCode,Code,Discount,ValidFrom,ValidTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] DiscountCode discountCode)
        {
            if (ModelState.IsValid)
            {
                discountCode.AddedDate = DateTime.Now;
                discountCode.AddedBy = "Admin";
                discountCode.IsActive = true;
                _context.Add(discountCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(discountCode);
        }

        // GET: DiscountCode/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DiscountCode == null)
            {
                return NotFound();
            }

            var discountCode = await _context.DiscountCode.FindAsync(id);
            if (discountCode == null)
            {
                return NotFound();
            }
            return View(discountCode);
        }

        // POST: DiscountCode/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDiscountCode,Code,Discount,ValidFrom,ValidTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] DiscountCode discountCode)
        {
            if (id != discountCode.IdDiscountCode)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(discountCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiscountCodeExists(discountCode.IdDiscountCode))
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
            return View(discountCode);
        }

        // GET: DiscountCode/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.DiscountCode == null)
            {
                return Problem("Entity set 'HotelContext.DiscountCode'  is null.");
            }
            var discountCode = await _context.DiscountCode.FindAsync(id);
            if (discountCode != null)
            {
                _context.DiscountCode.Remove(discountCode);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: DiscountCode/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DiscountCode == null)
            {
                return Problem("Entity set 'HotelContext.DiscountCode'  is null.");
            }
            var discountCode = await _context.DiscountCode.FindAsync(id);
            if (discountCode != null)
            {
                _context.DiscountCode.Remove(discountCode);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DiscountCodeExists(int id)
        {
          return (_context.DiscountCode?.Any(e => e.IdDiscountCode == id)).GetValueOrDefault();
        }
    }
}
