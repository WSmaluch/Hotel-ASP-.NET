using Hotel.Data;
using Hotel.Data.Data.CMS.About;
using Hotel.Data.Data.CMS.Attractions;
using Hotel.Data.Data.CMS.MainPage;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using System;
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

            ViewBag.Options = _context.Options
             .Where(option => option.IsActive && option.IdOption != 20)
             .Include(option => option.ContentItems)
             .OrderBy(option => Guid.NewGuid())
             .Take(3)
             .ToList();
            ViewBag.Types =
            (
                   from type in _context.Types
                   select type
                ).ToList();
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
            ViewBag.AboutPage = _context.AboutPage
         .Where(about => about.IsActive)
         .FirstOrDefault();

            ViewBag.AboutBanner = _context.AboutSilderPhoto
        .Where(aboutPhoto => aboutPhoto.IsActive)
        .ToList();


            ViewBag.AboutUs = _context.AboutPage
        .Where(about => about.IsActive)
        .ToList();

            return View();
        }


        public IActionResult Rooms()
        {
            ViewBag.Offers =
            (
                   from offer in _context.Offer
                   where offer.IsActive == true
                   select offer
                ).ToList();

            ViewBag.Types =
            (
                   from type in _context.Types
                   select type
                ).ToList();
            ViewBag.Options = _context.Options
             .Where(option => option.IsActive && option.IdOption != 20)
             .Include(option => option.ContentItems)
             .OrderBy(option => Guid.NewGuid())
             .Take(3)
             .ToList();


            return View();
        }

        public IActionResult Blog()
        {
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
            ViewBag.Contact = _context.ContactPage
         .Where(contact => contact.IsActive)
         .FirstOrDefault();
            return View();
        }

        public IActionResult Attractions()
        {
            var attractions = _context.Attraction
                .Where(att => att.IsActive)
                .Include(att => att.AttractionType)
                .Include(att => att.AttractionPrice)
                .OrderBy(att => Guid.NewGuid())
                .Take(4)
                .ToList();

            ViewBag.Attractions = attractions;
            return View();
        }

        public IActionResult Vouchers()
        {
            ViewBag.Vouchers = _context.Vouchers
                .Where(v => v.IsActive)
                .ToList();

            ViewBag.VoucherTypes = _context.VoucherType
                .Where(vt => vt.IsActive)
                .ToList();

            ViewBag.VoucherPrices = _context.VoucherPrice
                .Where(vp => vp.IsActive)
                .OrderByDescending(vp => vp.Amount)
                .ToList();

            return View(); 
        }

        public IActionResult SiteGuide()
        {
            ViewBag.SiteGuide = _context.SiteGuide
                .Where(v => v.IsActive)
                .OrderBy(att => Guid.NewGuid())
                .Take(3)
                .ToList();

            return View(); 
        }

        public IActionResult Gallery()
        {
            ViewBag.Gallery = _context.Gallery
                .Where(v => v.IsActive)
                .ToList();

            return View();
        }



        public IActionResult RoomTypeDetails(int typeId)
        {
            var type = _context.Types
             .Where(t => t.IsActive && t.IdType == typeId)
             .FirstOrDefault();

                ViewBag.Type = type;

                ViewBag.Fac = _context.Types
                 .Where(t => t.IsActive && t.IdType == typeId)
                 .Include(type => type.Facilities)
                 .FirstOrDefault().Facilities;


                ViewBag.PhotoURLs = type.PhotosURL.Split(';');

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