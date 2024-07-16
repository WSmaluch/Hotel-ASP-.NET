using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Restaurant;

namespace Hotel.Intranet.Controllers
{
    public class RestaurantPageController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;

        public RestaurantPageController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: RestaurantPage
        public async Task<IActionResult> Index()
        {
              return _context.RestaurantPage != null ? 
                          View(await _context.RestaurantPage.ToListAsync()) :
                          Problem("Entity set 'HotelContext.RestaurantPage'  is null.");
        }

        // GET: RestaurantPage/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RestaurantPage == null)
            {
                return NotFound();
            }

            var restaurantPage = await _context.RestaurantPage
                .FirstOrDefaultAsync(m => m.IdAboutPage == id);
            if (restaurantPage == null)
            {
                return NotFound();
            }

            return View(restaurantPage);
        }

        // GET: RestaurantPage/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RestaurantPage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdAboutPage,BannerUrl,BannerTitle,Content1Title,Content1Description,Content1Picture1Url,Content1Picture2Url,Content2Title,Content2Description,Content3Title,Content3Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantPage restaurantPage, IFormFile BannerUrl, IFormFile Content1Picture1Url, IFormFile Content1Picture2Url)
        {
            if (BannerUrl != null && BannerUrl.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(BannerUrl);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.BannerUrl = imageUrl;
            }
            else
            {
                restaurantPage.BannerUrl = "No photo";
            }

            if (Content1Picture1Url != null && Content1Picture1Url.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(Content1Picture1Url);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.Content1Picture1Url = imageUrl;
            }
            else
            {
                restaurantPage.Content1Picture1Url = "No photo";
            }

            if (Content1Picture2Url != null && Content1Picture2Url.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(Content1Picture2Url);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.Content1Picture2Url = imageUrl;
            }
            else
            {
                restaurantPage.Content1Picture2Url = "No photo";
            }


            restaurantPage.AddedBy = "Admin";
            restaurantPage.AddedDate = DateTime.Now;
            restaurantPage.IsActive = true;
            _context.Add(restaurantPage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: RestaurantPage/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RestaurantPage == null)
            {
                return NotFound();
            }

            var restaurantPage = await _context.RestaurantPage.FindAsync(id);
            if (restaurantPage == null)
            {
                return NotFound();
            }
            return View(restaurantPage);
        }

        // POST: RestaurantPage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdAboutPage,BannerUrl,BannerTitle,Content1Title,Content1Description,Content1Picture1Url,Content1Picture2Url,Content2Title,Content2Description,Content3Title,Content3Description,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] RestaurantPage restaurantPage, IFormFile BannerUrl, IFormFile Content1Picture1Url, IFormFile Content1Picture2Url)
        {
            if (id != restaurantPage.IdAboutPage)
            {
                return NotFound();
            }

            var existingOption = await _context.RestaurantPage.AsNoTracking().FirstOrDefaultAsync(o => o.IdAboutPage == id);

            if (existingOption == null)
            {
                return NotFound();
            }

            if (BannerUrl != null && BannerUrl.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(BannerUrl);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.BannerUrl = imageUrl;
            }
            else
            {
                // Zachowanie istniejącego PhotoUrl, jeśli nie przesłano nowego pliku - jest ten sam
                restaurantPage.BannerUrl = existingOption.BannerUrl;
            }

            if (Content1Picture1Url != null && Content1Picture1Url.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(Content1Picture1Url);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.Content1Picture1Url = imageUrl;
            }
            else
            {
                // Zachowanie istniejącego PhotoUrl, jeśli nie przesłano nowego pliku - jest ten sam
                restaurantPage.Content1Picture1Url = existingOption.Content1Picture1Url;
            }

            if (Content1Picture2Url != null && Content1Picture2Url.Length > 0)
            {
                // Przetwarzanie przesłanego pliku
                var imageService = new ImgurService(_configuration);
                var imageUrl = await imageService.UploadImageAsync(Content1Picture2Url);

                // Zapisanie linku do obrazu w obiekcie Types
                restaurantPage.Content1Picture2Url = imageUrl;
            }
            else
            {
                // Zachowanie istniejącego PhotoUrl, jeśli nie przesłano nowego pliku - jest ten sam
                restaurantPage.Content1Picture2Url = existingOption.Content1Picture2Url;
            }

            try
                {
                    _context.Update(restaurantPage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantPageExists(restaurantPage.IdAboutPage))
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

        // GET: RestaurantPage/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.RestaurantPage == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantPage'  is null.");
            }
            var restaurantPage = await _context.RestaurantPage.FindAsync(id);
            if (restaurantPage != null)
            {
                _context.RestaurantPage.Remove(restaurantPage);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: RestaurantPage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RestaurantPage == null)
            {
                return Problem("Entity set 'HotelContext.RestaurantPage'  is null.");
            }
            var restaurantPage = await _context.RestaurantPage.FindAsync(id);
            if (restaurantPage != null)
            {
                _context.RestaurantPage.Remove(restaurantPage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantPageExists(int id)
        {
          return (_context.RestaurantPage?.Any(e => e.IdAboutPage == id)).GetValueOrDefault();
        }
    }
}
