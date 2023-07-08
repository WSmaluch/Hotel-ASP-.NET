using Hotel.Data;
using Hotel.Data.Data.CMS.MainPage;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel.PortalWWW.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly HotelContext _context;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(HotelContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Banner =
            (
                   from banner in _context.Banner
                   where banner.IsActive == true
                   select banner
                ).ToList();

            ViewBag.Video =
           (
                  from video in _context.Video
                  where video.IsActive == true
                  select video
               ).ToList();

            return View();
        }

		public IActionResult About()
		{
			return View();
		}

        public IActionResult Rooms()
        {
            return View();
        }

        public IActionResult Blog()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}