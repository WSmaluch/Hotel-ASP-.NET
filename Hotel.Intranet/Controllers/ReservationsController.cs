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


        public IActionResult Create(DateTime? checkIn, DateTime? checkOut, int NumberOfAdults, int NumberOfChildren)
        {
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.NumberOfAdults = NumberOfAdults;
            ViewBag.NumberOfChildren = NumberOfChildren;

            var days = (checkOut.Value - checkIn.Value).TotalDays;


            ViewBag.Options = new SelectList(new List<SelectListItem>());

            if (checkIn.HasValue && checkOut.HasValue)
            {
                var availableOptions = _context.Options
                    .Where(o => o.StartDate <= checkIn && o.EndDate >= checkOut)
                    .ToList();

                ViewBag.Options = new SelectList(availableOptions.Select(o => new
                {
                    IdOption = o.IdOption,
                    DisplayText = $"{o.Name} - {(o.Price * days).ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}"
                }), "IdOption", "DisplayText");


            }

            if (checkIn.HasValue && checkOut.HasValue)
            {
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
                // Download all rooms if dates are not selected
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


        private string GenerateDiscountCode()
        {
            string discountCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            return discountCode;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateConfirmed([Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            // Creating a new discount code
            DiscountCode discountCode = new DiscountCode
            {
                Code = GenerateDiscountCode(), 
                Discount = 5,
                ValidFrom = DateTime.Today, 
                ValidTo = DateTime.Today.AddMonths(6), 
                IsActive = true, 
                AddedBy = "Admin", 
                AddedDate = DateTime.Now, 
            };

            _context.DiscountCode.Add(discountCode);

            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "PhotosURL", reservation.RoomId);
            reservation.AddedDate = DateTime.Now;
            reservation.AddedBy = "Admin";
            reservation.StatusId = 1;
            _context.Room.Find(reservation.RoomId).StatusId = 1;
            _context.Add(reservation);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm([Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            var roomType = _context.Room
            .Include(r => r.Type)
            .Where(r => r.IdRoom == reservation.RoomId)
            .Select(r => r.Type)
            .FirstOrDefault();

            if (roomType != null)
            {
                // Perform price calculations based on room data
                var price = (double)GetTotalPrice(roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren);
                reservation.TotalPrice = AddOccupancyFee(price, roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren) + CalculateOptionPrice(reservation);
            }
            else
            {
                // Error handling when room information could not be retrieved
                ModelState.AddModelError(string.Empty, "Error retrieving room information.");
                return View("Create", reservation); 
            }

            ViewBag.RoomNumber = _context.Room.Find(reservation.RoomId).Number;

            ViewBag.OptionName = _context.Options.Find(reservation.OptionId).Name;

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


			var availableOptions = _context.Options
					.Where(o => o.StartDate <= reservation.CheckIn&& o.EndDate >= reservation.CheckOut)
					.ToList();

			var days = (reservation.CheckOut - reservation.CheckIn).TotalDays;

			ViewBag.Options = new SelectList(availableOptions.Select(o => new
			{
				IdOption = o.IdOption,
				DisplayText = $"{o.Name} - {(o.Price * days).ToString("C", CultureInfo.CreateSpecificCulture("en-US"))}"
			}), "IdOption", "DisplayText");

			return View(reservation);
        }



        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdReservation,RoomId,Name,LastName,Email,PhoneNumber,City,AdressFirstLine,PostalCode,CheckIn,CheckOut,NumberOfAdults,NumberOfChildren,SpecialRequests,TotalPrice,OptionId,StatusId,AddedBy,AddedDate,ModifiedBy,ModifiedDate,RemovedBy,RemovedDate")] Reservation reservation)
        {
            if (id != reservation.IdReservation)
            {
                return NotFound();
            }


            var existingReservation = await _context.Reservations.AsNoTracking().FirstOrDefaultAsync(o => o.IdReservation == id);

            //liczenie ponowne ceny
            if (reservation.TotalPrice == existingReservation.TotalPrice)
            {
                var roomType = _context.Room
                .Include(r => r.Type)
                .Where(r => r.IdRoom == reservation.RoomId)
                .Select(r => r.Type)
                .FirstOrDefault();

                var price = (double)GetTotalPrice(roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren);
                reservation.TotalPrice = AddOccupancyFee(price, roomType.IdType, reservation.CheckIn, reservation.CheckOut, reservation.NumberOfAdults, reservation.NumberOfChildren) + CalculateOptionPrice(reservation);
            }



            ViewData["RoomId"] = new SelectList(_context.Room, "IdRoom", "PhotosURL", reservation.RoomId);
                try
                {
                    reservation.StatusId = existingReservation.StatusId;
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

            // Change your booking status to a new one
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
                // If there is a semicolon, get the fragment up to the first semicolon
                photoFragment = room.PhotosURL.Substring(0, indexOfSemicolon).Trim();
            }
            else
            {
                // If there is no semicolon, use the entire string
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

        private decimal GetTotalPrice(int typeId, DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            decimal totalPrice = 0;

            for (DateTime currentDay = checkIn; currentDay < checkOut; currentDay = currentDay.AddDays(1))
            {
                // Check which price list is current on a given day
                var roomPricing = _context.RoomPricing
                    .Where(rp => rp.TypeId == typeId && rp.ValidFrom <= currentDay && rp.ValidTo >= currentDay)
                    .FirstOrDefault();

                if (roomPricing != null)
                {
                    // Calculate the price for a given day based on the price list
                    decimal dailyPrice = CalculateDailyPrice(roomPricing, adults, children);
                    totalPrice += dailyPrice;
                }
                else
                {
                    // In this example, I assume that the missing prices are zeroed.
                    totalPrice += 0;
                }
            }

            return totalPrice;
        }

        private decimal CalculateDailyPrice(RoomPricing roomPricing, int adults, int children)
        {
            if (_context.Types.Find(roomPricing.TypeId).MaxAmountOfPeople == (adults + children))
            {
                // If the number of people matches the maximum capacity, add the basic and additional prices
                return roomPricing.BasePriceAdult + roomPricing.BasePriceChildren;
            }
            else
            {
                // If the number of people exceeds the available capacity, only add the base price
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
                occupancyFee = price * 0.05; //5% additional fee for occupancy below or equal to 30%
            }
            else if (occupancyPercentage <= 50)
            {
                occupancyFee = price * 0.1; // 10% additional fee for occupancy below or equal to 50%
            }
            else if (occupancyPercentage <= 70)
            {
                occupancyFee = price * 0.15; // 15% additional fee for occupancy below or equal to 70%
            }
            else
            {
                occupancyFee = price * 0.2; // 20% additional fee for occupancy above 70%
            }

            return price + occupancyFee;
        }

        private double CalculateOptionPrice(Reservation res)
        {
            var availableOptions = _context.Options
                    .Where(o => o.StartDate <= res.CheckIn && o.EndDate >= res.CheckOut)
                    .ToList();

            var days = (res.CheckOut - res.CheckIn).TotalDays;

            var option = availableOptions
                .Where(o => o.IdOption == res.OptionId)
                .FirstOrDefault();

            var price = option.Price * days;

            return price;
        }
    }
}
