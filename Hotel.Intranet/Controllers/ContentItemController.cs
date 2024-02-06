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
    public class ContentItemController : Controller
    {
        private readonly HotelContext _context;

        public ContentItemController(HotelContext context)
        {
            _context = context;
        }

        // GET: ContentItem
        public async Task<IActionResult> Index()
        {
              return _context.ContentItem != null ? 
                          View(await _context.ContentItem.ToListAsync()) :
                          Problem("Entity set 'HotelContext.ContentItem'  is null.");
        }

        // GET: ContentItem/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ContentItem == null)
            {
                return NotFound();
            }

            var contentItem = await _context.ContentItem
                .FirstOrDefaultAsync(m => m.IdContentItem == id);
            if (contentItem == null)
            {
                return NotFound();
            }

            return View(contentItem);
        }

        // GET: ContentItem/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContentItem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdContentItem,Title,Description,IconUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] ContentItem contentItem)
        {
            //if (ModelState.IsValid)
            //{
                contentItem.AddedBy = "Admin";
                contentItem.AddedDate= DateTime.Now;
                contentItem.IsActive = true;
                _context.Add(contentItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            //}
            return View(contentItem);
        }

        // GET: ContentItem/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ContentItem == null)
            {
                return NotFound();
            }

            var contentItem = await _context.ContentItem.FindAsync(id);
            if (contentItem == null)
            {
                return NotFound();
            }
            return View(contentItem);
        }

        // POST: ContentItem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdContentItem,Title,Description,IconUrl,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] ContentItem contentItem)
        {
            if (id != contentItem.IdContentItem)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contentItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentItemExists(contentItem.IdContentItem))
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
            return View(contentItem);
        }

        // GET: ContentItem/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ContentItem == null)
            {
                return NotFound();
            }

            var contentItem = await _context.ContentItem
                .FirstOrDefaultAsync(m => m.IdContentItem == id);
            if (contentItem == null)
            {
                return NotFound();
            }

            return View(contentItem);
        }

        // POST: ContentItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ContentItem == null)
            {
                return Problem("Entity set 'HotelContext.ContentItem'  is null.");
            }
            var contentItem = await _context.ContentItem.FindAsync(id);
            if (contentItem != null)
            {
                _context.ContentItem.Remove(contentItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentItemExists(int id)
        {
          return (_context.ContentItem?.Any(e => e.IdContentItem == id)).GetValueOrDefault();
        }
    }
}
