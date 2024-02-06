using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Hotel.Data;
using Hotel.Data.Data.Booking;
using System.Globalization;

namespace Hotel.Intranet.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HotelContext _context;
        private decimal totalPrice;
        private decimal TotalPrice { get => totalPrice; set => totalPrice = value; }
        public ReservationsController(HotelContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var hotelContext = _context.Reservations.Include(r => r.Room).Include(r => r.ReservationStatus).Include(r => r.Option);
            return View(await hotelContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ReservationStatus)
                .FirstOrDefaultAsync(m => m.IdReservation == id);
            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public IActionResult CreateReservationDate()
        {
            return View();
        }


        //// GET: Reservations/Create
        //public IActionResult Create()
        //{
        //    ViewBag.Rooms = new SelectList(_context.Room.Include(r => r.Type).Select(r => new {
        //        IdRoom = r.IdRoom,
        //        DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
        //    }), "IdRoom", "DisplayNameWithNumber");
        //    return View();
        //}

        //// GET: Reservations/Create
        ////private List<Room> GetAvailableRooms(DateTime checkIn, DateTime checkOut, int numberOfAdults, int numberOfChildren)
        ////{
        ////    var reservedRoomIds = _context.Reservations
        ////        .Where(r => (checkIn < r.CheckOut && checkOut > r.CheckIn) || (checkIn >= r.CheckOut && checkOut <= r.CheckIn))
        ////        .Select(r => r.RoomId)
        ////        .ToList();

        ////    var availableRooms = _context.Room
        ////        .Include(r => r.Type)
        ////        .Where(r => !reservedRoomIds.Contains(r.IdRoom))
        ////        .ToList();

        ////    // Filtruj pokoje na podstawie liczby osób
        ////    availableRooms = availableRooms
        ////        .Where(r => r.Type.MaxAmountOfPeople >= (numberOfAdults + numberOfChildren))
        ////        .ToList();

        ////    return availableRooms;


        ////    dsadasdasdasdsadasdasdasdasdsa

        ////        var availableRooms = _context.Room
        ////    .Where(room =>
        ////        !_context.Reservations
        ////            .Where(reservation => reservation.IsActive == true &&
        ////                reservation.CheckIn <= checkOut &&
        ////                reservation.CheckOut >= checkIn)
        ////            .Select(reservation => reservation.RoomId)
        ////            .Contains(room.IdRoom) &&
        ////        room.Type.MaxAmountOfPeople >= (numberOfAdults + numberOfChildren))
        ////    .ToList();

        ////    return availableRooms;
        ////}


        public IActionResult Create(DateTime? checkIn, DateTime? checkOut, int NumberOfAdults, int NumberOfChildren)
        {
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.NumberOfAdults = NumberOfAdults;
            ViewBag.NumberOfChildren = NumberOfChildren;

            var days = (checkOut.Value - checkIn.Value).TotalDays;


            // Inicjalizacja ViewBag.Options, nawet jeśli brak dostępnych opcji
            ViewBag.Options = new SelectList(new List<SelectListItem>());

            if (checkIn.HasValue && checkOut.HasValue)
            {
                // Pobierz dostępne opcje
                var availableOptions = _context.Options
                    .Where(o => o.StartDate <= checkIn && o.EndDate >= checkOut)
                    .ToList();

                // Zaktualizuj ViewBag.Options z rzeczywistymi opcjami
                ViewBag.Options = new SelectList(availableOptions.Select(o => new
                {
                    IdOption = o.IdOption,
                    DisplayText = $"{o.Name} - {(o.Price * days).ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}"
                }), "IdOption", "DisplayText");


            }

            if (checkIn.HasValue && checkOut.HasValue)
            {
                // Pobierz dostępne pokoje
                //ViewBag.Rooms = new SelectList(_context.Room.Include(r => r.Type)
                //    .Where(r => r.StatusId == 9 && !_context.Reservations
                //        .Where(reservation => reservation.IsActive && reservation.StatusId != 9)
                //        .Select(reservation => reservation.RoomId)
                //        .Contains(r.IdRoom))
                //    .Select(r => new
                //    {
                //        IdRoom = r.IdRoom,
                //        DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
                //    }), "IdRoom", "DisplayNameWithNumber");

                //    ViewBag.Rooms = new SelectList(
                //_context.Room
                //    .Where(r => !_context.Reservations
                //        .Where(reservation => reservation.IsActive && (reservation.StatusId == 1 || reservation.StatusId == 2) &&
                //            reservation.CheckIn <= checkOut && reservation.CheckOut >= checkIn)
                //        .Select(reservation => reservation.RoomId)
                //        .Contains(r.IdRoom))
                //    .Select(r => new
                //    {
                //        IdRoom = r.IdRoom,
                //        DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
                //    })
                //    .ToList(), "IdRoom", "DisplayNameWithNumber");
                ViewBag.Rooms = new SelectList(
        _context.Room
            .Where(r => !_context.Reservations
                .Where(reservation => reservation.IsActive && (reservation.StatusId == 1 || reservation.StatusId == 2) &&
                    reservation.CheckIn <= checkOut && reservation.CheckOut >= checkIn)
                .Select(reservation => reservation.RoomId)
                .Contains(r.IdRoom) &&
                r.Type.MaxAmountOfPeople >= (NumberOfAdults + NumberOfChildren))
            .Select(r => new
            {
                IdRoom = r.IdRoom,
                DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
            })
            .ToList(), "IdRoom", "DisplayNameWithNumber");

            }
            else
            {
                // Pobierz wszystkie pokoje, jeśli daty nie zostały wybrane
                ViewBag.Rooms = new SelectList(_context.Room.Include(r => r.Type)
                    .Where(r => r.StatusId == 9 && !_context.Reservations
                        .Where(reservation => reservation.IsActive && reservation.StatusId != 9)
                        .Select(reservation => reservation.RoomId)
                        .Contains(r.IdRoom))
                    .Select(r => new
                    {
                        IdRoom = r.IdRoom,
                        DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
                    }), "IdRoom", "DisplayNameWithNumber");
            }

            return View();
        }


        //// POST: Reservations/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        //{
        //    //if (ModelState.IsValid)
        //    //{
        //        reservation.AddedDate = DateTime.Now;
        //        reservation.AddedBy = "Admin";
        //        reservation.StatusId = 1;
        //        _context.Room.Find(reservation.RoomId).StatusId = 1;
        //        _context.Add(reservation);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    //}
        //    ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "PhotosURL", reservation.RoomId);
        //    return View(reservation);
        //}

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            //if (ModelState.IsValid)
            //{
            reservation.AddedDate = DateTime.Now;
            reservation.AddedBy = "Admin";
            reservation.StatusId = 1;
            _context.Room.Find(reservation.RoomId).StatusId = 1;
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //}
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "PhotosURL", reservation.RoomId);
            return View(reservation);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm([Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            //reservation.TotalPrice = CalculateFinalPrice(_context.Types.Where(r=> r.Rooms.Find(reservation.RoomId)), typeId, SetcheckIn.ToString(), SetcheckOut.ToString(), adults, children); ;

            var roomType = _context.Room
            .Include(r => r.Type)
            .Where(r => r.IdRoom == reservation.RoomId)
            .Select(r => r.Type)
            .FirstOrDefault();

            if (roomType != null)
            {
                // Przeprowadź obliczenia ceny na podstawie danych pokoju
                var price = (double)GetTotalPrice(roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren);
                reservation.TotalPrice = AddOccupancyFee(price, roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren);
            }
            else
            {
                // Obsługa błędu, gdy nie udało się pobrać informacji o pokoju
                ModelState.AddModelError(string.Empty, "Error retrieving room information.");
                return View("Create", reservation); // Przekieruj z powrotem do formularza z błędem
            }

            return View("Confirm", reservation);
        }

        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.Rooms = new SelectList(_context.Room.Include(r => r.Type).Select(r => new
            {
                IdRoom = r.IdRoom,
                DisplayNameWithNumber = $"{r.Number} - {r.Type.Name}"
            }), "IdRoom", "DisplayNameWithNumber", reservation.RoomId);

            return View(reservation);
        }



        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReservation,RoomId,Name,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,IsActive,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            if (id != reservation.IdReservation)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.IdReservation))
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
            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "PhotosURL", reservation.RoomId);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'HotelContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'HotelContext.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);

                _context.Room.Find(reservation.RoomId).StatusId = 9;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.IdReservation == id)).GetValueOrDefault();
        }

        // GET: Reservations/ChangeStatus/5
        public async Task<IActionResult> ChangeStatus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Pobierz dostępne statusy z bazy danych i przekaż do widoku
            ViewBag.Statuses = await _context.Status.Where(s => s.StatusId >= 1 && s.StatusId <= 6).ToListAsync();

            return View(reservation);
        }

        // POST: Reservations/ChangeStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, int newStatusId)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Zmień status rezerwacji na nowy
            reservation.StatusId = newStatusId;
            if (newStatusId == 6)
            {
                _context.Room.Find(reservation.RoomId).StatusId = 8;
            }
            else if (newStatusId == 5)
            {
                reservation.IsActive = false;
                _context.Room.Find(reservation.RoomId).StatusId = 9;
            }
            else
                _context.Room.Find(reservation.RoomId).StatusId = newStatusId;

            // Zapisz zmiany
            _context.Update(reservation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("/GenerateInvoice")]
        public IActionResult GenerateInvoice(int reservationId)
        {
            var res = _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ReservationStatus)
                .Include(r => r.Option)
                .SingleOrDefault(r => r.IdReservation == reservationId);

            var room = _context.Types.Find(_context.Room.Find(res.RoomId).TypeId);

            var option = _context.Options.Find(res.OptionId);


            string photoFragment;
            int indexOfSemicolon = option.PhotoUrl.IndexOf(';');

            if (indexOfSemicolon > -1)
            {
                // Jeśli istnieje średnik, pobierz fragment do pierwszego średnika
                photoFragment = room.PhotosURL.Substring(0, indexOfSemicolon).Trim();
            }
            else
            {
                // Jeśli nie ma średnika, użyj całego ciągu
                photoFragment = room.PhotosURL.Trim();
            }


            string pdfHtml = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Hotel Trapise Invoice</title>
    <style>
        body {
            font-family: Arial, sans-serif;
        }

        .invoice {
            width: 80%;
            margin: 20px auto;
            padding: 20px;
        }

        .invoice-header {
            text-align: center;
            margin-bottom: 20px;
        }

        .customer-info {
            margin-top: 20px;
            margin-bottom: 20px;
            width: 60%;
            float: left;
        }

        .invoice-details {
            margin-top: 20px;
            width: 40%; 
            float: left;
        }

        .invoice-table-container {
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-radius: 15px; 
            border: 1px solid;
        }

        .invoice-table {
            width: 60%;
            border-collapse: collapse;
            margin-top: 20px;
            border-radius: 15px;
        }

        .invoice-table th, .invoice-table td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        .invoice-total {
            margin-top: 20px;
            text-align: right;
        }

        .room-details {
            width: 50%;
            text-align: left;
        }

        .room-image {
            max-width: 100%;
            border-radius: 15px;
        }
        img 
        {
            border-top-right-radius: 15px;
            border-bottom-right-radius: 15px;
            margin-bottom: -5px;
        }
    </style>
</head>
<body>
    <div class='invoice'>
        
        <div style='width: 100%; float: left;'>
        
            <div class='invoice-header' style='width: 30%; float: left;' >
                <h2>Hotel Stay Invoice</h2>
                <p>Issue Date: " + DateTime.Now.ToString("dd/MM/yyyy") + @"</p>
            </div>
            <img src='https://i.imgur.com/EYUZVwh.png' style='width: 20%; float:right;'/>

        </div>

        <div id='invoice-body'>

            <div class='customer-info'>
                <h3>Customer Information</h3>
                <p>Name: " + res.Name + ' ' + res.LastName + @"</p>
                <p>Address: " + res.AdressFirstLine + @", " + res.City + @"</p>
                <p>Phone number: " + res.PhoneNumber + @"</p>
                <p>E-mail: " + res.Email + @"</p>
            </div>

            <div class='invoice-details'>
                <h3>Stay Details</h3>
                <p>Reservation Number: " + res.IdReservation + @"</p>
                <p>Check-in Date: " + res.CheckIn.ToString("dd/MM/yyyy") + @"</p>
                <p>Check-out Date: " + res.CheckOut.ToString("dd/MM/yyyy") + @"</p>
                <p>Number of Adults: " + res.NumberOfAdults + @"</p>
                <p>Number of Children: " + res.NumberOfChildren + @"</p>
            </div>
        </div>

        <div class='invoice-table-container' style='margin-top:10%'>
            
            <div class='room-details'>
                <h3>Room Details</h3>
                <p>Type: " + room.Name + @"</p>
                <p>Option: " + option.Name + @"</p>
                <b><p>Price: " + res.TotalPrice + @" USD / " + (res.CheckOut - res.CheckIn).TotalDays + @" days</p></b>
            </div>
            <div class='room-details'>
                <img style='width: 100%; height: 100%;' src=" + photoFragment + @">
            </div>
        </div>
        <br/>
        <br/>
        <br/>
        <div class='invoice-total'>
            <p>Signature</p>
            <br/>
            <p>..................</p>
        </div>
    </div>
</body>
</html>

        ";
            var renderer = new IronPdf.HtmlToPdf();
            renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4Small;
            renderer.PrintOptions.MarginTop = -13;
            renderer.PrintOptions.MarginBottom = -13;
            renderer.PrintOptions.MarginLeft = -13;
            renderer.PrintOptions.MarginRight = -13;

            var pdf = renderer.RenderHtmlAsPdf(pdfHtml);

            var fileContents = pdf.BinaryData;
            return File(fileContents, "application/pdf", "Invoice.pdf");
        }


        //public decimal CalculateFinalPrice(int typeId, string checkIn, string checkOut, int adults, int children)
        //{
        //    var availableRooms = GetAvailableRooms(DateTime.Parse(checkIn), DateTime.Parse(checkOut), adults,children);

        //    var selectedRoom = availableRooms.FirstOrDefault();

        //    int totalRooms = 0;

        //    foreach (var item in _context.Room)
        //    {
        //        if (item.TypeId == typeId)
        //        {
        //            totalRooms++;
        //        }
        //    }

        //    int occupancy = totalRooms - availableRooms.Count;
        //    double occupancyPercentage = 0;

        //    if (totalRooms > 0)
        //    {
        //        occupancyPercentage = ((double)occupancy / totalRooms) * 100;
        //    }

        //    decimal BasePriceAdult = GetTotalPrice(typeId, DateTime.Parse(checkIn), DateTime.Parse(checkOut), adults, children);

        //    // Doliczenie opłaty za obłożenie w zależności od procentowego obłożenia
        //    decimal occupancyFee = 0;

        //    if (occupancyPercentage <= 30)
        //    {
        //        occupancyFee = (decimal)((double)BasePriceAdult * 0.05); //5% dodatkowej opłaty dla obłożenia poniżej lub równego 30%
        //    }
        //    else if (occupancyPercentage <= 50)
        //    {
        //        occupancyFee = (decimal)((double)BasePriceAdult * 0.1); // 10% dodatkowej opłaty dla obłożenia poniżej lub równego 50%
        //    }
        //    else if (occupancyPercentage <= 70)
        //    {
        //        occupancyFee = (decimal)((double)BasePriceAdult * 0.15); // 15% dodatkowej opłaty dla obłożenia poniżej lub równego 70%
        //    }
        //    else
        //    {
        //        occupancyFee = (decimal)((double)BasePriceAdult * 0.2); // 20% dodatkowej opłaty dla obłożenia powyżej 70%
        //    }

        //    return TotalPrice = BasePriceAdult + occupancyFee;
        //}

        //private decimal GetTotalPrice(int typeId, DateTime checkIn, DateTime checkOut, int adults, int children)
        //{
        //    // Find the applicable room pricing based on the room type and date range
        //    var roomPricing = _context.RoomPricing
        //        .Where(rp => rp.TypeId == typeId && rp.ValidFrom <= checkIn && rp.ValidTo >= checkOut)
        //        .FirstOrDefault();

        //    if (roomPricing == null)
        //    {
        //        // Handle the case when there's no applicable pricing
        //        return 0; // You can choose to return some default value or handle it differently
        //    }

        //    // Calculate the number of nights
        //    int numberOfNights = (int)(checkOut - checkIn).TotalDays;

        //    // Calculate the total price based on room pricing and duration
        //    if (_context.Types.Find(typeId).MaxAmountOfPeople == (adults + children))
        //    {
        //        var over = adults + children - _context.Types.Find(typeId).MaxAmountOfPeople;
        //        TotalPrice = (decimal)(((roomPricing.BasePriceAdult) + (roomPricing.BasePriceChildren)) * numberOfNights);
        //    }
        //    else
        //    {
        //        TotalPrice = roomPricing.BasePriceAdult * numberOfNights;
        //    }

        //    return TotalPrice;
        //}


        private decimal GetTotalPrice(int typeId, DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            decimal totalPrice = 0;

            for (DateTime currentDay = checkIn; currentDay < checkOut; currentDay = currentDay.AddDays(1))
            {
                // Sprawdź, który cennik jest aktualny w danym dniu
                var roomPricing = _context.RoomPricing
                    .Where(rp => rp.TypeId == typeId && rp.ValidFrom <= currentDay && rp.ValidTo >= currentDay)
                    .FirstOrDefault();

                if (roomPricing != null)
                {
                    // Oblicz cenę za dany dzień na podstawie cennika
                    decimal dailyPrice = CalculateDailyPrice(roomPricing, adults, children);
                    totalPrice += dailyPrice;
                }
                else
                {
                    // W tym przykładzie zakładam, że brakujące ceny są zerowane.
                    totalPrice += 0;
                }
            }

            return totalPrice;
        }

        private decimal CalculateDailyPrice(RoomPricing roomPricing, int adults, int children)
        {
            if (_context.Types.Find(roomPricing.TypeId).MaxAmountOfPeople == (adults + children))
            {
                // Jeśli liczba osób zgadza się z maksymalną liczbą miejsc, dodaj cenę podstawową i dodatkową
                return roomPricing.BasePriceAdult + roomPricing.BasePriceChildren;
            }
            else
            {
                // Jeśli liczba osób przekracza dostępną liczbę miejsc, dodaj tylko cenę podstawową
                return roomPricing.BasePriceAdult;
            }
        }

        private double AddOccupancyFee(double price, int IdType, DateTime CheckIn, DateTime CheckOut, int NumberOfAdults, int NumberOfChildrens)
        {
            var totalRooms = _context.Room.Count();

            var availableRooms = _context.Room
                .Where(room => 
                    !_context.Reservations
                        .Where(reservation => reservation.IsActive == true && (reservation.StatusId == 1 || reservation.StatusId == 3 ) &&
                            reservation.CheckIn <= CheckOut &&
                            reservation.CheckOut >= CheckIn)
                        .Select(reservation => reservation.RoomId)
                        .Contains(room.IdRoom))
                .ToList();

            int occupancy = totalRooms - availableRooms.Count;

            double occupancyPercentage = 0;

            if (totalRooms > 0)
            {
                occupancyPercentage = ((double)occupancy / totalRooms) * 100;
            }

            double occupancyFee = 0;

            if (occupancyPercentage <= 30)
            {
                occupancyFee = price * 0.05; //5% dodatkowej opłaty dla obłożenia poniżej lub równego 30%
            }
            else if (occupancyPercentage <= 50)
            {
                occupancyFee = price * 0.1; // 10% dodatkowej opłaty dla obłożenia poniżej lub równego 50%
            }
            else if (occupancyPercentage <= 70)
            {
                occupancyFee = price * 0.15; // 15% dodatkowej opłaty dla obłożenia poniżej lub równego 70%
            }
            else
            {
                occupancyFee = price * 0.2; // 20% dodatkowej opłaty dla obłożenia powyżej 70%
            }

            return price + occupancyFee;
        }
    }
}
