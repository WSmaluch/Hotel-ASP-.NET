using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Newsletter;

namespace Hotel.Intranet.Controllers
{
    public class NewsletterConfigController : Controller
    {
        private readonly HotelContext _context;

        public NewsletterConfigController(HotelContext context)
        {
            _context = context;
        }

        // GET: NewsletterConfig
        public async Task<IActionResult> Index()
        {
              return _context.NewsletterConfig != null ? 
                          View(await _context.NewsletterConfig.ToListAsync()) :
                          Problem("Entity set 'HotelContext.NewsletterConfig'  is null.");
        }

        // GET: NewsletterConfig/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NewsletterConfig == null)
            {
                return NotFound();
            }

            var newsletterConfig = await _context.NewsletterConfig
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsletterConfig == null)
            {
                return NotFound();
            }

            return View(newsletterConfig);
        }

        // GET: NewsletterConfig/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsletterConfig/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Content,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] NewsletterConfig newsletterConfig)
        {
            if (ModelState.IsValid)
            {
                newsletterConfig.AddedDate = DateTime.Now;
                newsletterConfig.AddedBy = "Admin";
                _context.Add(newsletterConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsletterConfig);
        }

        // GET: NewsletterConfig/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NewsletterConfig == null)
            {
                return NotFound();
            }

            var newsletterConfig = await _context.NewsletterConfig.FindAsync(id);
            if (newsletterConfig == null)
            {
                return NotFound();
            }
            return View(newsletterConfig);
        }

        // POST: NewsletterConfig/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] NewsletterConfig newsletterConfig)
        {
            if (id != newsletterConfig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsletterConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsletterConfigExists(newsletterConfig.Id))
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
            return View(newsletterConfig);
        }

        // GET: NewsletterConfig/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.NewsletterConfig == null)
            {
                return Problem("Entity set 'HotelContext.NewsletterConfig'  is null.");
            }
            var nc = await _context.NewsletterConfig.FindAsync(id);
            if (nc != null)
            {
                _context.NewsletterConfig.Remove(nc);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: NewsletterConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NewsletterConfig == null)
            {
                return Problem("Entity set 'HotelContext.NewsletterConfig'  is null.");
            }
            var newsletterConfig = await _context.NewsletterConfig.FindAsync(id);
            if (newsletterConfig != null)
            {
                _context.NewsletterConfig.Remove(newsletterConfig);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsletterConfigExists(int id)
        {
          return (_context.NewsletterConfig?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
