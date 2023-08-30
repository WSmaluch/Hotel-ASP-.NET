using Hotel.Data;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotel.PortalWWW.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelContext _context;

        public BookingController(HotelContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? checkin, DateTime? checkout, int adults, int children)
        {
            DateTime checkIn = checkin.HasValue ? checkin.Value.Date : DateTime.Today;
            DateTime checkOut = checkout.HasValue ? checkout.Value.Date : (DateTime.Today).AddDays(7);

            var model = new BookingModel
            {
                facilities = await _context.Facilities.ToListAsync(),
                types = await _context.Types.ToListAsync(),
                rooms = await _context.Room.ToListAsync()
            };
            ViewBag.CheckIn = checkIn.ToShortDateString();
            ViewBag.CheckOut = checkOut.ToShortDateString();
            ViewBag.Adults = adults;
            ViewBag.Children = children;
            return View(model);
        }
        public async Task<IActionResult> Form(int roomId, DateTime? checkin, DateTime? checkout, int adults, int children)
        {
            DateTime checkIn = checkin.HasValue ? checkin.Value.Date : DateTime.Today;
            DateTime checkOut = checkout.HasValue ? checkout.Value.Date : (DateTime.Today);

            ViewBag.RoomId = roomId;

            var model = new BookingModel
            {
                facilities = await _context.Facilities.ToListAsync(),
                types = await _context.Types.ToListAsync(),
                rooms = await _context.Room.ToListAsync()
            };

            ViewBag.CheckIn = checkIn.ToShortDateString();
            ViewBag.CheckOut = checkOut.ToShortDateString();
            ViewBag.Adults = adults;
            ViewBag.Children = children;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Submit(BookingModel formData)
        {
            //if (ModelState.IsValid)
            //{
            //    // Przetwarzanie i zapis danych do bazy danych
            //    // ...

            //    ViewBag.Message = "Reservation submitted successfully!";
            //    return View("Confirmation"); // Widok potwierdzenia rezerwacji
            //}

            //// Jeśli dane są niepoprawne, wróć do formularza z wyświetleniem błędów
            return View();
        }
    }
}
