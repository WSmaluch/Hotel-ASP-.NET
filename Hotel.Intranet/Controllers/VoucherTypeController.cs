using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Voucher;

namespace Hotel.Intranet.Controllers
{
    public class VoucherTypeController : Controller
    {
        private readonly HotelContext _context;

        public VoucherTypeController(HotelContext context)
        {
            _context = context;
        }

        // GET: VoucherType
        public async Task<IActionResult> Index()
        {
              return _context.VoucherType != null ? 
                          View(await _context.VoucherType.ToListAsync()) :
                          Problem("Entity set 'HotelContext.VoucherType'  is null.");
        }

        // GET: VoucherType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VoucherType == null)
            {
                return NotFound();
            }

            var voucherType = await _context.VoucherType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherType == null)
            {
                return NotFound();
            }

            return View(voucherType);
        }

        // GET: VoucherType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoucherType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] VoucherType voucherType)
        {
            voucherType.AddedBy = "Admin";
            voucherType.AddedDate = DateTime.Now;
            voucherType.IsActive = true;
                _context.Add(voucherType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: VoucherType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VoucherType == null)
            {
                return NotFound();
            }

            var voucherType = await _context.VoucherType.FindAsync(id);
            if (voucherType == null)
            {
                return NotFound();
            }
            return View(voucherType);
        }

        // POST: VoucherType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] VoucherType voucherType)
        {
            if (id != voucherType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucherType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherTypeExists(voucherType.Id))
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
            return View(voucherType);
        }

        // GET: VoucherType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.VoucherType == null)
            {
                return Problem("Entity set 'HotelContext.VoucherType'  is null.");
            }
            var voucherType = await _context.VoucherType.FindAsync(id);
            if (voucherType != null)
            {
                _context.VoucherType.Remove(voucherType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: VoucherType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VoucherType == null)
            {
                return Problem("Entity set 'HotelContext.VoucherType'  is null.");
            }
            var voucherType = await _context.VoucherType.FindAsync(id);
            if (voucherType != null)
            {
                _context.VoucherType.Remove(voucherType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherTypeExists(int id)
        {
          return (_context.VoucherType?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
