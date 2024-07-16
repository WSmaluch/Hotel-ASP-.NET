using Hotel.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.PortalWWW.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;

        public RestaurantController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult MainPage()
        {

            ViewBag.RestaurantPage = _context.RestaurantPage
                .Where(v => v.IsActive).FirstOrDefault();

            ViewBag.Gallery = _context.Gallery
                .Where(v => v.IsActive)
                .ToList();

            ViewBag.Dishes = _context.Dish
                .Include(d => d.Category)
                .Include(d => d.Price)
                .Include(d => d.Ingredients)
                .Where(v => v.IsActive)
                .ToList();


            //seasonal menu
            // Pobierz wszystkie sezony menu do wyboru na stronie
            var seasonalMenus = _context.SeasonalMenu.ToList();

            // Pobierz pierwszy sezonowy menu do wyświetlenia domyślnie
            var defaultSeasonalMenu = seasonalMenus.FirstOrDefault();

            // Pobierz dania dla pierwszego sezonowego menu (lub jakiegoś domyślnego)
            var dishes = _context.Dish
                                .Include(d => d.Category)
                                .Include(d => d.Price)
                                .Include(d => d.Ingredients)
                                .ToList();

            ViewBag.SeasonalMenus = seasonalMenus;
            ViewBag.Dishes = dishes;
            //

            return View();
        }
    }
}
