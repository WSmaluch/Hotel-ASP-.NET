using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Hotel.PortalWWW.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        private decimal totalPrice;

        private decimal TotalPrice { get => totalPrice; set => totalPrice = value; }

        public BookingController(HotelContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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

            //to liczy ile jest pokojów kazdego typu
            var roomCounts = await _context.Room
                .GroupBy(rc => rc.TypeId)
                .Select(roomGroup => new
                {
                    TypeId = roomGroup.Key,
                    RoomCount = roomGroup.Count()
                })
                .ToListAsync();

            //to liczy ile pokojow kazdego typu jest zarezerwowanych w danym okresie czasu (CZYLI NIE MOZNA ICH ZAREZERWOWAC)
            var reservedRoomCounts = await _context.Reservations
                .Where(reservation => reservation.CheckIn <= SetcheckOut && reservation.CheckOut >= SetcheckIn && reservation.IsActive)
                .Join(
                    _context.Room,
                    reservation => reservation.RoomId,
                    room => room.IdRoom,
                    (reservation, room) => new { TypeId = room.TypeId }
                )
                .GroupBy(item => item.TypeId)
                .Select(g => new
                {
                    TypeId = g.Key,
                    ReservedRoomCount = g.Count()
                })
                .ToListAsync();

            //zapytanie łączące dwa powyższe
            var availableRoomTypeIds = roomCounts
                .GroupJoin(
                    reservedRoomCounts,
                    rc => rc.TypeId,
                    rrc => rrc.TypeId,
                    (rc, r) => new
                    {
                        TypeId = rc.TypeId,
                        RoomCount = rc.RoomCount,
                        ReservedRoomCount = r.FirstOrDefault()?.ReservedRoomCount ?? 0
                    })
                .Where(result => result.RoomCount - result.ReservedRoomCount > 0)
                .Select(result => result.TypeId)
                .ToList();

            //konwersja int na types, łączy z tabelą Facilities aby wybrac nalezace do typu i sprawdza czy podana ilosc ludzi sie zmieści
            var availableRoomTypes = await _context.Types
           .Where(type => availableRoomTypeIds.Contains(type.IdType) && type.MaxAmountOfPeople >= adults + children)
           .Include(type => type.Facilities) // Include facilities for each type
           .ToListAsync();

            if (availableRoomTypes.Count == 0)
            {
                ViewBag.Message = "No available rooms";
            }

            var model = new BookingModel
            {
                Options = await _context.Options.Include(o => o.ContentItems).ToListAsync(),
                types = availableRoomTypes,
                PricesByRoomType = new Dictionary<int, decimal>() 
            };

            // Oblicz ceny dla poszczególnych typów pokoi
            foreach (var roomType in availableRoomTypes)
            {
                int typeId = roomType.IdType;
                decimal totalPrice = GetTotalPrice(typeId, SetcheckIn, SetcheckOut, adults, children);
                model.PricesByRoomType.Add(typeId, totalPrice);
            }

            ViewBag.CheckIn = SetcheckIn.ToString("yyyy/MM-dd");
            ViewBag.CheckOut = SetcheckOut.ToString("yyyy/MM-dd");
            ViewBag.Adults = adults;
            ViewBag.Children = children;

            return View(model);
        }

        public decimal TotalPriceOfBooking(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var days = (checkOut - checkIn).Days;
            var totalPrice = 111;
            return totalPrice;
        }
        public async Task<IActionResult> Form(int typeId, DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            var model = new BookingModel
            {
                Options = await _context.Options.Include(o => o.ContentItems).Where(o => o.IsActive).ToListAsync(),
                facilities = await _context.Facilities.ToListAsync(),
                types = await _context.Types.ToListAsync(),
                rooms = await _context.Room.ToListAsync()
            };

            var days = (checkOut - checkIn).Days;

            ViewBag.TypeId = typeId;
            ViewBag.CheckIn = checkIn;
            ViewBag.CheckOut = checkOut;
            ViewBag.Adults = adults;
            ViewBag.Children = children;
            ViewBag.Price = TotalPriceOfBooking(typeId, checkIn, checkOut);


            return View(model);
        }

        private decimal GetTotalPrice(int typeId, DateTime checkIn, DateTime checkOut, int adults, int children)
        {
            // Find the applicable room pricing based on the room type and date range
            var roomPricing = _context.RoomPricing
                .Where(rp => rp.TypeId == typeId && rp.ValidFrom <= checkIn && rp.ValidTo >= checkOut)
                .FirstOrDefault();

            if (roomPricing == null)
            {
                // Handle the case when there's no applicable pricing
                return 0; // You can choose to return some default value or handle it differently
            }

            // Calculate the number of nights
            int numberOfNights = (int)(checkOut - checkIn).TotalDays;

            // Calculate the total price based on room pricing and duration
            if (_context.Types.Find(typeId).MaxAmountOfPeople < (adults + children))
            {
                var over = adults + children - _context.Types.Find(typeId).MaxAmountOfPeople;
                TotalPrice = (decimal)((double)(roomPricing.BasePriceAdult * adults) + (double)(roomPricing.BasePriceChildren * children) + ((double)(roomPricing.BasePriceAdult * over) * 0.5)) * numberOfNights;
            }
            else
            {
                TotalPrice = (roomPricing.BasePriceAdult * adults + roomPricing.BasePriceChildren * children) * numberOfNights;
            }

            return TotalPrice;
        }

        public List<Room> GetAvailableRooms(int typeId, string checkIn, string checkOut)
        {
            DateTime checkInDate = DateTime.Parse(checkIn);
            DateTime checkOutDate = DateTime.Parse(checkOut);

            var availableRooms = _context.Room
                .Where(room => room.TypeId == typeId &&
                    !_context.Reservations
                        .Where(reservation => reservation.IsActive == true &&
                            reservation.CheckIn <= checkOutDate &&
                            reservation.CheckOut >= checkInDate)
                        .Select(reservation => reservation.RoomId)
                        .Contains(room.IdRoom))
                .ToList();

            return availableRooms;
        }


        public decimal CalculateFinalPrice(int typeId, string checkIn, string checkOut, int adults, int children)
        {
            var availableRooms = GetAvailableRooms(typeId, checkIn, checkOut);

            var selectedRoom = availableRooms.FirstOrDefault();

            int totalRooms = 0;
            foreach (var item in _context.Room)
            {
                if (item.TypeId == typeId)
                {
                    totalRooms++;
                }
            }

            int occupancy = totalRooms - availableRooms.Count;
            double occupancyPercentage = 0;

            if (totalRooms > 0)
            {
                occupancyPercentage = ((double)occupancy / totalRooms) * 100;
            }

            decimal basePrice = GetTotalPrice(typeId, DateTime.Parse(checkIn), DateTime.Parse(checkOut), adults, children);

            // Doliczenie opłaty za obłożenie w zależności od procentowego obłożenia
            decimal occupancyFee = 0;

            if (occupancyPercentage <= 30)
            {
                occupancyFee = (decimal)((double)basePrice * 0.05); //5% dodatkowej opłaty dla obłożenia poniżej lub równego 30%
            }
            else if (occupancyPercentage <= 50)
            {
                occupancyFee = (decimal)((double)basePrice * 0.1); // 10% dodatkowej opłaty dla obłożenia poniżej lub równego 50%
            }
            else if (occupancyPercentage <= 70)
            {
                occupancyFee = (decimal)((double)basePrice * 0.15); // 15% dodatkowej opłaty dla obłożenia poniżej lub równego 70%
            }
            else
            {
                occupancyFee = (decimal)((double)basePrice * 0.2); // 20% dodatkowej opłaty dla obłożenia powyżej 70%
            }

            return TotalPrice = basePrice + occupancyFee;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(
            int typeId,
            string checkIn,
            string checkOut,
            int adults,
            int children,
            string name,
            string specialRequests,
            string email)
        {
            try
            {
                var availableRooms = GetAvailableRooms(typeId, checkIn, checkOut);

                // Tworzenie obiektu rezerwacji na podstawie przekazanych danych
                var reservation = new Reservation
                {
                    RoomId = availableRooms.FirstOrDefault().IdRoom,
                    CheckIn = DateTime.Parse(checkIn),
                    CheckOut = DateTime.Parse(checkOut),
                    NumberOfAdults = adults,
                    NumberOfChildren = children,
                    Name = name,
                    SpecialRequests = specialRequests,
                    TotalPrice = (double)CalculateFinalPrice(typeId, checkIn, checkOut, adults, children)
                };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                int confirmationCode = reservation.IdReservation;
                await SendConfirmationEmail(reservation, email, confirmationCode);
                ViewBag.Message = "Reservation submitted successfully!";
                ViewBag.TypeOfRoom = _context.Types.Find(_context.Room.Find(reservation.RoomId).TypeId);
                ViewBag.Reservation = reservation;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Message = "An error occurred while processing your reservation.";
                return View();
            }
        }

        private async Task SendConfirmationEmail(Reservation reservation, string email, int confirmationCode)
        {
            var smtpSettings = _configuration.GetSection("SmtpSettings");

            var smtpClient = new SmtpClient(smtpSettings["SmtpServer"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"])
            };

            var room = await _context.Room
        .Where(r => r.IdRoom == reservation.RoomId)
        .FirstOrDefaultAsync();

            var typeOfRoom = _context.Types.Find(_context.Room.Find(reservation.RoomId).TypeId);



            var message = new MailMessage
            {
                From = new MailAddress("hoteltest321@outlook.com"),
                Subject = "Reservation Confirmation",
                IsBodyHtml = true,
            };
            message.To.Add(email);

            message.Body = @"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Reservation Confirmation at the Hotel</title>
                <style>
                    body {
                        font-family: 'Helvetica Neue', sans-serif;
                        background-color: lightgray;
                        margin: 0;
                        padding: 0;
                        display: flex;
                        flex-direction: column;
                    }

                    #header {
                        background-color: #DBDFE2;
                        color: black;
                        overflow: hidden;
                        border-top-left-radius: 15px;
                        border-top-right-radius: 15px; 
                        width: 60%;
                        margin-left: 20%;
                    }

                    #logo {
                        float: left;
                        width: 50%;
                        padding-left: 10%;
                    }

                    #contact {
                        float: left;
                        padding-left: 20%;
                        text-align: right;
                        color: #21464E;
                    }

                    @media screen and (max-width: 768px) {
                    #logo {
                        width: 100%;
                        padding-left: 0;
                        text-align: center;
                    }

                    #contact {
                        display: none;
                    }
                }

                    #content {
                        background-color: lightgray;
                        color: white;
            
            
                    }

                    #left {
                        width: 20%;
                        float: left;
                        min-height: 1px;
            
                    }

                    #main {
                        width: 60%;
                        float: left;
                        background-color: white;
                        color: black;
                        text-align: center;
                    }
                    #main table{
                        text-align: left;
                    }
                    #main p{
                        text-align: center;
                    }

                    #right {
                        width: 20%;
                        float: left;
            
                    }

                    img {
                        max-width: 100%;
                        height: auto;
                    }
                    #mainLogo{
                        padding-top: 3%;
                        width: 100px;

                    }

                    h1 {
                        color: #0E3F3C;
                        font-size: 28px;
                    }

                    p {
                        font-size: 16px;
                        line-height: 1.6;
                        text-align: left;
                        margin: 10px 0;
                    }
                    #description{
                        padding: 0 15% 0;
                    }
                    #photo{
                        padding: 3%;
                        background-color: #F6F5F3;
                        border-radius: 15px; 
                        width: 52%; 
                        margin-top: 20px;
                        margin-left: 20%;
                        margin-bottom: 3%;
                    }
                    #icon{
                        width:30px;
                        height:30px;
                    }
                    #footer {
                        background-color: #DBDFE2;
                        color: black;
                        text-align: center;
                        border-bottom-left-radius: 15px;
                        border-bottom-right-radius: 15px;
                        margin-left: 20%;
                        width: 60%;
            
                        padding-top: 30px;
                    }
                    #buttonAllInfo {
                        display: inline-block;
                        margin-right: 10px;
                        border: 2px solid #02464F;
                        background-color: #02464F;
                        color: white;
                        padding: 14px 28px;
                        font-size: 16px;
                        cursor: pointer;
                        text-align: center;
                        border-radius: 15px;
                        text-decoration: none;
                    }

                    #buttonDownload {
                        display: inline-block;
                        border: 2px solid #02464F;
                        color: #02464F;
                        padding: 14px 28px;
                        font-size: 16px;
                        cursor: pointer;
                        text-align: center;
                        border-radius: 15px;
                        text-decoration: none;
                    }
                    #warning{
                        font-size: small;
                        padding: 0 10% 0;
                        color: rgb(99, 98, 98);
                    }
                </style>
            </head>
            <body>
                <div id='header'>
                    <div id='logo'>
                       <!-- <img src='https://i.imgur.com/rlyrnOK.png' alt='Logo'>-->
                        <!-- <img src='https://i.imgur.com/ZFJmvms.png' alt='Logo'> -->
                        <img src='https://i.imgur.com/SkKxLYR.png' alt='Logo'>
            
                    </div>
                    <div id='contact'>
                        <p>+48 333 333 333</p>
                        <p>hotel@hotel.pl</p>
                    </div>
                </div>
                <div id='content'>
                    <div id='left'></div>
                    <div id='main'>
                        <img src='https://i.imgur.com/IxtZwtE.png' alt='Logo' id='mainLogo'>
                        <h1>Your dream apartment awaits!</h1>
                        <p>Hello " + reservation.Name + "!" + @"</p>
                        <p id='description'>Congratulations on your recent purchase of the beautiful apartment located at Trapise Resort.<br/><br/>
                            We're thrilled that you've found the perfect room to suit your needs and preferences. Our team is dedicated to ensuring your stay is as comfortable and enjoyable as possible. 
                            If you have any special requests or need assistance with anything during your stay, please don't hesitate to reach out to our concierge services.<br/></p>
                            <div id='photo'>
                                <img id='roomphoto' style='border-radius: 15px;' src=" + typeOfRoom.PhotosURL + @" alt='Room photo'>
                                <table style='color:#0E3F3C'> 
                                    <tr>
                                       <!-- <td width='90%'><h3>" + room.IdRoom + @"</h3></td> -->
                                        <td width='90%'><h3>" + typeOfRoom.Name + @"</h3></td>
                                        <td><h3>" + "$" + reservation.TotalPrice.ToString() + "/" + (reservation.CheckOut - reservation.CheckIn).Days + "days" + @"</h3></td>
                                    </tr>
                                </table>
                                <table style='float: left;'>
                                    <tr height='60px'>
                                        <td><img id='icon' src='https://i.imgur.com/1wdIpUZ.png' alt='human'>" + reservation.NumberOfAdults.ToString() + "adult(s)" + (reservation.NumberOfChildren > 0 ? reservation.NumberOfChildren.ToString() + "children(s)" : "") + @"</td>
                                    </tr>
                                    <tr style='float: left;'>
                                        <td width='130px'><img id='icon' src='https://i.imgur.com/WxRri4Y.png' alt='shower'> 1</td>
                                        <td width='130px'><img id='icon' src='https://i.imgur.com/CZdNj90.png' alt='bed'> 1</td>
                                        <td width='130px'><img id='icon' src='https://i.imgur.com/8ETVSV2.png' alt='size'> 1</td>
                                    </tr>
                                </table>
                                <br>
                                <div style='margin-top: 100px;'>
                                    <hr>
                                    <a href=''><button id='buttonAllInfo' style='display: inline-block; margin-right: 10px;' onclick='showAllInfo()'>All Info</button></a>
                                    <a href='https://drive.google.com/file/d/1JWQweTBgC1wTOHquws4UjKBKmOXglUaa/view?usp=sharing' download><button id='buttonDownload' style='display: inline-block;'>Download Agreement</button></a>
                                </div>
                            </div>
                    </div>
                    <div id='right'></div>
                </div>
                <div id='footer'>
                    Do you have any questions?<br>
                    <a href='https://www.apple.com/app-store/'><img style='padding-top: 2%;' src='https://i.imgur.com/mrveqmz.png' width='135' height='40'></a>
                    <a href='https://play.google.com/'><img src='https://i.imgur.com/15YmLZV.png' width='135' height='40'></a>
                    <a href='https://consumer.huawei.com/pl/mobileservices/appgallery/'><img src='https://i.imgur.com/FeKjbqK.png' width='135' height='40'></a>
                    <p id='warning'>Please be cautious of fraudulent messages. This is an automated message, and we kindly ask you not to respond.
                        For more information about how we handle your personal data, including your rights, please refer to our privacy policy.
                        The data controller for your personal information is Hotel Trapise, located at 4 Colonge Street, 32-863 Czchów. For further details, please visit our website at hoteltrapise.com.</p>
                </div>
            </body>
            </html>
    ";
            await smtpClient.SendMailAsync(message);
        }
    }
}
