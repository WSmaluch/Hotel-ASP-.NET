using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Attractions;

namespace Hotel.Intranet.Controllers
{
    public class AttractionTypeController : Controller
    {
        private readonly HotelContext _context;

        public AttractionTypeController(HotelContext context)
        {
            _context = context;
        }

        // GET: AttractionType
        public async Task<IActionResult> Index()
        {
              return _context.AttractionType != null ? 
                          View(await _context.AttractionType.ToListAsync()) :
                          Problem("Entity set 'HotelContext.AttractionType'  is null.");
        }

        // GET: AttractionType/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AttractionType == null)
            {
                return NotFound();
            }

            var attractionType = await _context.AttractionType
                .FirstOrDefaultAsync(m => m.AttractionTypeId == id);
            if (attractionType == null)
            {
                return NotFound();
            }

            return View(attractionType);
        }

        // GET: AttractionType/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttractionType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AttractionTypeId,Name,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] AttractionType attractionType)
        {
            attractionType.AddedBy = "Admin";
            attractionType.AddedDate = DateTime.Now;
            attractionType.IsActive = true;
            _context.Add(attractionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            return View(attractionType);
        }

        // GET: AttractionType/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AttractionType == null)
            {
                return NotFound();
            }

            var attractionType = await _context.AttractionType.FindAsync(id);
            if (attractionType == null)
            {
                return NotFound();
            }
            return View(attractionType);
        }

        // POST: AttractionType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AttractionTypeId,Name,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] AttractionType attractionType)
        {
            if (id != attractionType.AttractionTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attractionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttractionTypeExists(attractionType.AttractionTypeId))
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
            return View(attractionType);
        }

        // GET: AttractionType/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.AttractionType == null)
            {
                return Problem("Entity set 'HotelContext.AttractionType'  is null.");
            }
            var at = await _context.AttractionType.FindAsync(id);
            if (at != null)
            {
                _context.AttractionType.Remove(at);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: AttractionType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.AttractionType == null)
            {
                return Problem("Entity set 'HotelContext.AttractionType'  is null.");
            }
            var attractionType = await _context.AttractionType.FindAsync(id);
            if (attractionType != null)
            {
                _context.AttractionType.Remove(attractionType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttractionTypeExists(int id)
        {
          return (_context.AttractionType?.Any(e => e.AttractionTypeId == id)).GetValueOrDefault();
        }
    }
}
