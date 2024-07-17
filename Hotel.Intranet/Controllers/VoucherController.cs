using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Voucher;

namespace Hotel.Intranet.Controllers
{
    public class VoucherController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;

        public VoucherController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Voucher
        public async Task<IActionResult> Index()
        {
              return _context.Vouchers != null ? 
                          View(await _context.Vouchers.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Vouchers'  is null.");
        }

        // GET: Voucher/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voucher == null)
            {
                return NotFound();
            }

            return View(voucher);
        }

        // GET: Voucher/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Voucher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageUrl,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Voucher voucher, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                // Processing the uploaded file
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Saving a link to string
                voucher.ImageUrl = imageUrl;
            }
            else
            {
                voucher.ImageUrl = "No photo";
            }

            voucher.AddedBy = "Admin";
            voucher.AddedDate = DateTime.Now;
            voucher.IsActive = true;
                _context.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Voucher/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vouchers == null)
            {
                return NotFound();
            }

            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null)
            {
                return NotFound();
            }
            return View(voucher);
        }

        // POST: Voucher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageUrl,Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Voucher voucher, IFormFile photoFile)
        {
            if (id != voucher.Id)
            {
                return NotFound();
            }

            var existingOption = await _context.Vouchers.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id);

            if (existingOption == null)
            {
                return NotFound();
            }

            if (photoFile != null && photoFile.Length > 0)
            {
                // Processing the uploaded file
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Saving a link to string
                voucher.ImageUrl = imageUrl;
            }
            else
            {
                // Retention of existing PhotoUrl if no new file uploaded - it is the same
                voucher.ImageUrl = existingOption.ImageUrl;
            }

                try
                {
                    _context.Update(voucher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VoucherExists(voucher.Id))
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

        // GET: Voucher/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'HotelContext.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Voucher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vouchers == null)
            {
                return Problem("Entity set 'HotelContext.Vouchers'  is null.");
            }
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher != null)
            {
                _context.Vouchers.Remove(voucher);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VoucherExists(int id)
        {
          return (_context.Vouchers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
