
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
            

            var seasonalMenus = _context.SeasonalMenu
            .Include(sm => sm.Dishes)
            .ThenInclude(d => d.Price) // Include Price if needed
            .Where(sm => sm.StartDate <= DateTime.Now && sm.EndDate >= DateTime.Now)
            .ToList();

            // Pass the seasonal menus to the view
            ViewBag.SeasonalMenusName = seasonalMenus.Select(sm => sm.Name).FirstOrDefault();
            ViewBag.SeasonalMenus = seasonalMenus;


           
            var dishes = _context.Dish
                                .Include(d => d.Category)
                                .Include(d => d.Price)
                                .Include(d => d.Ingredients)
                                .ToList();

            ViewBag.Dishes = dishes;

            return View();
        }
        public ActionResult DishDetails(int id)
        {
            // Retrieve a dish from the database based on id
            var dish = _context.Dish.Include(d => d.Category)
                                .Include(d => d.Price)
                                .Include(d => d.Ingredients).FirstOrDefault(d => d.Id == id);

            if (dish == null)
            {
                return View("Error404");
            }

            ViewBag.Gallery = dish.ImageUrl;
            
            ViewBag.DishName = dish.Name;
            ViewBag.DishDescription = dish.Description;
            ViewBag.DishPrice = dish.Price.Price;

            ViewBag.DishIngredients = dish.Ingredients;

            return View(); 
        }

    }
}
