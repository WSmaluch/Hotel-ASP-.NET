using Hotel.Data;
using Hotel.Data.Data.CMS.About;
using Hotel.Data.Data.CMS.MainPage;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Hotel.PortalWWW.Controllers
{
    public class HomeController : BaseController
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public HomeController(HotelContext context) : base(context)
        {
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
            ViewBag.AboutUs =
            (
                   from about in _context.AboutPage
                   where about.IsActive == true
                   select about
                ).ToList().FirstOrDefault();
            ViewBag.Offers =
            (
                   from offer in _context.Offer
                   where offer.IsActive == true
                   select offer
                ).ToList();

            return View();
        }

        public IActionResult About()
        {
            var aboutPage = _context.AboutPage.FirstOrDefault(); //important
            ViewBag.AboutUs =
            (
                   from about in _context.AboutPage
                   where about.IsActive == true
                   select about
                ).ToList().FirstOrDefault();
            ViewBag.AboutBanner =
            (
                   from aboutPhoto in _context.AboutSilderPhoto
                   where aboutPhoto.IsActive == true
                   select aboutPhoto
                ).ToList();

            return View(aboutPage);
        }


        public IActionResult Rooms()
        {
            ViewBag.Offers =
            (
                   from offer in _context.Offer
                   where offer.IsActive == true
                   select offer
                ).ToList();
            return View();
        }

        public IActionResult Blog()
        {
            //var blogPage = _context.Post;
            ViewBag.Posts =
            (
                   from post in _context.Post
                   where post.IsActive == true
                   select post
                ).ToList();
            return View();
        }

        public IActionResult Contact()
        {
            var contactpage = _context.ContactPage.FirstOrDefault(); //important
            return View(contactpage);
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