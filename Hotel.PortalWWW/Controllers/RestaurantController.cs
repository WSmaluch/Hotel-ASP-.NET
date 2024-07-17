
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

            ViewBag.Gallery = _context.Promotion
                .Where(v => v.IsActive)
                .Where(v=> v.StartDate <= DateTime.Now && v.EndDate >= DateTime.Now)
                .ToList();

            ViewBag.Dishes = _context.Dish
                .Include(d => d.Category)
                .Include(d => d.Price)
                .Include(d => d.Ingredients)
                .Where(v => v.IsActive)
                .ToList();

            var activeMenus = _context.Menu
            .Include(m => m.Dishes)
            .ThenInclude(d => d.Price)
            .Where(m => m.IsActive && m.StartDate <= DateTime.Now && m.EndDate >= DateTime.Now)
            .ToList();

            ViewBag.Menu = activeMenus;
            //ViewBag.Menu = activeMenu?.Dishes.ToList();
            //ViewBag.MenuName = activeMenu?.Name;

            //seasonal menu
            // Pobierz wszystkie sezony menu do wyboru na stronie
            //var seasonalMenus = _context.SeasonalMenu
            //    .Include(sm => sm.Dishes)
            //    .Where(sm => sm.StartDate <= DateTime.Now.AddYears(1) && sm.EndDate >= DateTime.Now.AddYears(1))
            //    .ToList();

            //var seasonalMenuDishes = seasonalMenus.Select(sm => sm.Dishes.ToList()).ToList();

            //ViewBag.SeasonalMenus = seasonalMenuDishes;

            var seasonalMenus = _context.SeasonalMenu
            .Include(sm => sm.Dishes)
            .ThenInclude(d => d.Price) // Include Price if needed
            .Where(sm => sm.StartDate <= DateTime.Now && sm.EndDate >= DateTime.Now)
            .ToList();

            // Pass the seasonal menus to the view
            ViewBag.SeasonalMenusName = seasonalMenus.Select(sm => sm.Name).FirstOrDefault();
            ViewBag.SeasonalMenus = seasonalMenus;


            // Pobierz pierwszy sezonowy menu do wyświetlenia domyślnie
            //var defaultSeasonalMenu = seasonalMenus.FirstOrDefault();

            // Pobierz dania dla pierwszego sezonowego menu (lub jakiegoś domyślnego)
            var dishes = _context.Dish
                                .Include(d => d.Category)
                                .Include(d => d.Price)
                                .Include(d => d.Ingredients)
                                .ToList();

            ViewBag.Dishes = dishes;
            //

            return View();
        }
        public ActionResult DishDetails(int id)
        {
            // Pobierz danie z bazy danych na podstawie id
            var dish = _context.Dish.Include(d => d.Category)
                                .Include(d => d.Price)
                                .Include(d => d.Ingredients).FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return View("Error404"); // Zwróć 404 Not Found z opcjonalnym komunikatem
            }

            ViewBag.Gallery = dish.ImageUrl;
            // Przekazanie danych do ViewBag
            ViewBag.DishName = dish.Name;
            ViewBag.DishDescription = dish.Description;
            ViewBag.DishPrice = dish.Price.Price;

            ViewBag.DishIngredients = dish.Ingredients;

            return View(); // Zwróć widok DishDetails.cshtml
        }

    }
}
