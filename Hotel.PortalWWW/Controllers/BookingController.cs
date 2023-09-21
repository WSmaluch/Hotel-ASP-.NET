using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;

namespace Hotel.PortalWWW.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelContext _context;

        public BookingController(HotelContext context)
        {
            _context = context;
        }

        
        private (DateTime checkIn, DateTime checkOut) CalculateCheckInCheckOut(DateTime? checkin, DateTime? checkout)
        {
            DateTime SetcheckIn = checkin ?? DateTime.Today;
            DateTime SetcheckOut = checkout ?? checkin.Value.AddDays(7);
            return (SetcheckIn, SetcheckOut);
        }
        public async Task<IActionResult> Index(DateTime? checkIn, DateTime? checkOut, int adults, int children)
        {
            (DateTime SetcheckIn, DateTime SetcheckOut) = CalculateCheckInCheckOut(checkIn, checkOut);
            

            // Pobierz listę pokoi, które są dostępne w wybranym okresie
            var availableRooms = await _context.Room
                .Where(room => !_context.Reservations.Any(reservation =>
                    reservation.RoomId == room.IdRoom &&
                    reservation.IsActive &&
                    reservation.CheckIn <= checkOut &&
                    reservation.CheckOut >= checkIn))
                .ToListAsync();

            var model = new BookingModel
            {
                facilities = await _context.Facilities.ToListAsync(),
                types = await _context.Types.ToListAsync(),
                rooms = availableRooms
            };

            //ViewBag.CheckIn = checkIn;
            ViewBag.CheckIn = checkIn.Value.ToString("yyyy/MM-dd");
            ViewBag.CheckOut = checkOut.Value.ToString("yyyy/MM-dd");
            ViewBag.Adults = adults;
            ViewBag.Children = children;

            return View(model);
        }
        public async Task<IActionResult> Form(int roomId, DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            var model = new BookingModel
            {
                facilities = await _context.Facilities.ToListAsync(),
                types = await _context.Types.ToListAsync(),
                rooms = await _context.Room.ToListAsync()
            };

            var days = (checkOut - checkIn).Days;

            ViewBag.RoomId = roomId;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Adults = adults;
            ViewBag.Children = children;
            ViewBag.Price = days * (_context.Room.Find(roomId).Price);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Submit(
            int roomId,
            string checkIn,
            string checkOut,
            int adults,
            int children,
            string name,
            string specialRequests)
        {
            try
            {
                // Tworzenie obiektu rezerwacji na podstawie przekazanych danych
                var reservation = new Reservation
                {
                    RoomId = roomId,
                    CheckIn = DateTime.Parse(checkIn),
                    CheckOut = DateTime.Parse(checkOut),
                    NumberOfAdults = adults,
                    NumberOfChildren = children,
                    Name = name,
                    SpecialRequests = specialRequests
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                //Przekierowanie użytkownika do widoku potwierdzenia

               ViewBag.Message = "Reservation submitted successfully!";
                //ViewBag.Message = reservation.Room.RoomNumber.ToString();
                return View();
            }
            catch (Exception ex)
            {
                // Obsługa błędów, np. w przypadku problemów z bazą danych
                ViewBag.Message = "An error occurred while processing your reservation.";
                return View(); // Wróć do formularza z wyświetleniem błędów
            }

            
        }
    }
}
