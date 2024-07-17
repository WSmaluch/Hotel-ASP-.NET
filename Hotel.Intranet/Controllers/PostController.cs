using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.CMS.Blog;

namespace Hotel.Intranet.Controllers
{
    public class PostController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        public PostController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Post
        public async Task<IActionResult> Index()
        {
              return _context.Post != null ? 
                          View(await _context.Post.ToListAsync()) :
                          Problem("Entity set 'HotelContext.Post'  is null.");
        }

        // GET: Post/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post
                .FirstOrDefaultAsync(m => m.IdPost == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Post/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPost,Title,Content,PhotoUrl,Category,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Post post, IFormFile photoFile)
        {
            if (photoFile != null && photoFile.Length > 0)
            {
                // Processing the uploaded file
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(photoFile);

                // Saving a link to string
                post.PhotoUrl = imageUrl;
            }
            else
            {
                post.PhotoUrl = "No photo";
            }
                post.AddedBy = "Admin";
            post.AddedDate = DateTime.Now;
            post.IsActive = true;
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Post/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        // POST: Post/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPost,Title,Content,PhotoUrl,Category,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Post post, IFormFile photoFile)
        {
            if (id != post.IdPost)
            {
                return NotFound();
            }

            var existingOption = await _context.Post.AsNoTracking().FirstOrDefaultAsync(o => o.IdPost == id);

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
                post.PhotoUrl = imageUrl;
            }
            else
            {
                // Retention of existing PhotoUrl if no new file uploaded - it is the same
                post.PhotoUrl = existingOption.PhotoUrl;
            }

            try
                {
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.IdPost))
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

        // GET: Post/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'HotelContext.Post'  is null.");
            }
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Post/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Post == null)
            {
                return Problem("Entity set 'HotelContext.Post'  is null.");
            }
            var post = await _context.Post.FindAsync(id);
            if (post != null)
            {
                _context.Post.Remove(post);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
          return (_context.Post?.Any(e => e.IdPost == id)).GetValueOrDefault();
        }
    }
}
