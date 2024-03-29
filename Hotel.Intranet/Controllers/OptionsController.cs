﻿using System;
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
    public class OptionsController : Controller
    {
        private readonly HotelContext _context;

        public OptionsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Options
        public async Task<IActionResult> Index()
        {
            var contentItem = await _context.Options.Include(o => o.ContentItems).ToListAsync();
            return View(contentItem);
        }

        // GET: Options/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Options == null)
            {
                return NotFound();
            }

            var options = await _context.Options
                .Include(o => o.ContentItems)
                .FirstOrDefaultAsync(m => m.IdOption == id);
            if (options == null)
            {
                return NotFound();
            }

            return View(options);
        }

        // GET: Options/Create
        public IActionResult Create()
        {
            ViewData["ContentItems"] = new SelectList(_context.ContentItem, "IdContentItem", "Title");
            return View();
        }

        // POST: Options/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdOption,Name,PhotoUrl,Price,StartDate,EndDate,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Options options, List<int> ContentItems, IFormFile photoFile)
        {
			//if (ModelState.IsValid)
			//{
			if (photoFile != null && photoFile.Length > 0)
			{
				// Przetwarzanie przesłanego pliku
				var imageService = new ImgurService();
				var imageUrl = await imageService.UploadImageAsync(photoFile);

				// Zapisanie linku do obrazu w obiekcie Types
				options.PhotoUrl = imageUrl;
			}

			if (ContentItems != null)
            {
                foreach (var contentItemId in ContentItems)
                {
                    var contentItem = await _context.ContentItem.FindAsync(contentItemId);
                    if (contentItem != null)
                    {
                        options.ContentItems.Add(contentItem);
                    }
                }
                options.AddedDate = DateTime.Now;
                options.AddedBy = "Admin";
                _context.Add(options);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(options);
            //}
        }

        // GET: Options/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Options == null)
            {
                return NotFound();
            }

            var options = await _context.Options.Include(r => r.ContentItems).FirstOrDefaultAsync(r => r.IdOption == id);

            if (options == null)
            {
                return NotFound();
            }

            var allFacilities = await _context.ContentItem.ToListAsync();

            var selectedContent = options.ContentItems.ToList();

            ViewBag.Content = selectedContent;

            ViewData["ContentItems"] = new SelectList(_context.ContentItem, "IdContentItem", "Title");
            return View(options);
        }

        // POST: Options/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdOption,Name,PhotoUrl,Price,StartDate,EndDate,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Options options)
        {
            if (id != options.IdOption)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(options);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OptionsExists(options.IdOption))
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
            return View(options);
        }

        // GET: Options/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null || _context.Options == null)
            //{
            //    return NotFound();
            //}

            //var options = await _context.Options
            //    .FirstOrDefaultAsync(m => m.IdOption == id);
            //if (options == null)
            //{
            //    return NotFound();
            //}

            //return View(options);

            if (_context.Options == null)
            {
                return Problem("Entity set 'HotelContext.Options'  is null.");
            }
            var options = await _context.Options.FindAsync(id);
            if (options != null)
            {
                _context.Options.Remove(options);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Options == null)
            {
                return Problem("Entity set 'HotelContext.Options'  is null.");
            }
            var options = await _context.Options.FindAsync(id);
            if (options != null)
            {
                _context.Options.Remove(options);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OptionsExists(int id)
        {
          return (_context.Options?.Any(e => e.IdOption == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> EditContentItems(int? id)
        {
            if (id == null || _context.Options == null)
            {
                return NotFound();
            }

            var options = await _context.Options
                .Include(t => t.ContentItems)
                .FirstOrDefaultAsync(t => t.IdOption == id);

            if (options == null)
            {
                return NotFound();
            }

            ViewBag.ContentItems = new SelectList(_context.ContentItem, "IdContentItem", "Title");


            return View(options);
        }

        [HttpPost]
        public IActionResult AddContentItems(int optionId, List<int> contentItemIds)
        {
            var room = _context.Options.Include(r => r.ContentItems).FirstOrDefault(r => r.IdOption == optionId);

            room.ContentItems.Clear();

            if (room != null)
            {
                foreach (var facilityId in contentItemIds)
                {
                    var contentItemToAdd = _context.ContentItem.FirstOrDefault(f => f.IdContentItem == facilityId);
                    if (contentItemToAdd != null && !room.ContentItems.Contains(contentItemToAdd))
                    {
                        room.ContentItems.Add(contentItemToAdd);
                    }
                }

                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Edit), new { id = optionId });
        }
    }
}
