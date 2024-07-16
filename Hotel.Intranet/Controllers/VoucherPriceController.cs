using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Voucher;

namespace Hotel.Intranet.Controllers
{
    public class VoucherPriceController : Controller
    {
        private readonly HotelContext _context;

        public VoucherPriceController(HotelContext context)
        {
            _context = context;
        }

        // GET: VoucherPrice
        public async Task<IActionResult> Index()
        {
              return _context.VoucherPrice != null ? 
                          View(await _context.VoucherPrice.ToListAsync()) :
                          Problem("Entity set 'HotelContext.VoucherPrice'  is null.");
        }

        // GET: VoucherPrice/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.VoucherPrice == null)
            {
                return NotFound();
            }

            var voucherPrice = await _context.VoucherPrice
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucherPrice == null)
            {
                return NotFound();
            }

            return View(voucherPrice);
        }

        // GET: VoucherPrice/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: VoucherPrice/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,ValidFrom,ValidTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] VoucherPrice voucherPrice)
        {
                voucherPrice.AddedBy = "Admin";
                voucherPrice.AddedDate = DateTime.Now;
                voucherPrice.IsActive = true;
                _context.Add(voucherPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: VoucherPrice/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.VoucherPrice == null)
            {
                return NotFound();
            }

            var voucherPrice = await _context.VoucherPrice.FindAsync(id);
            if (voucherPrice == null)
            {
                return NotFound();
            }
            return View(voucherPrice);
        }

        // POST: VoucherPrice/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,ValidFrom,ValidTo,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] VoucherPrice voucherPrice)
        {
            if (id != voucherPrice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voucherPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherPriceExists(voucherPrice.Id))
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
            return View(voucherPrice);
        }

        // GET: VoucherPrice/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.VoucherPrice == null)
            {
                return Problem("Entity set 'HotelContext.VoucherPrice'  is null.");
            }
            var voucherPrice = await _context.VoucherPrice.FindAsync(id);
            if (voucherPrice != null)
            {
                _context.VoucherPrice.Remove(voucherPrice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: VoucherPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VoucherPrice == null)
            {
                return Problem("Entity set 'HotelContext.VoucherPrice'  is null.");
            }
            var voucherPrice = await _context.VoucherPrice.FindAsync(id);
            if (voucherPrice != null)
            {
                _context.VoucherPrice.Remove(voucherPrice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherPriceExists(int id)
        {
          return (_context.VoucherPrice?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
