using Hotel.Data;
using Hotel.Data.Data.Booking;
using Hotel.PortalWWW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Net.Mail;
using System.Net;

namespace Hotel.PortalWWW.Controllers
{
    public class BookingController : Controller
    {
        private readonly HotelContext _context;
        private readonly IConfiguration _configuration;
        private double totalPrice { get; set; }

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
        public decimal TotalPriceOfBooking(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var days = (checkOut - checkIn).Days;
            var totalPrice = days * _context.Room.Find(roomId).Price;
            return totalPrice;
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
            ViewBag.Price = TotalPriceOfBooking(roomId,checkIn,checkOut);
            //ViewBag.Price = days * (_context.Room.Find(roomId).Price);
            //totalPrice = (double)(days * _context.Room.Find(roomId).Price);
            

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
            string specialRequests,
            string email)
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
                    SpecialRequests = specialRequests,
                    TotalPrice = (double)TotalPriceOfBooking(roomId, DateTime.Parse(checkIn), DateTime.Parse(checkOut))

            };

                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                int confirmationCode = reservation.IdReservation;
                await SendConfirmationEmail(reservation, email, confirmationCode);
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
                        <p>Hello " + reservation.Name+"!"+ @"</p>
                        <p id='description'>Congratulations on your recent purchase of the beautiful apartment located at Trapise Resort.<br/><br/>
                            We're thrilled that you've found the perfect room to suit your needs and preferences. Our team is dedicated to ensuring your stay is as comfortable and enjoyable as possible. 
                            If you have any special requests or need assistance with anything during your stay, please don't hesitate to reach out to our concierge services.<br/></p>
                            <div id='photo'>
                                <img id='roomphoto' style='border-radius: 15px;' src=" + room.PhotosURL + @" alt='Room photo'>
                                <table style='color:#0E3F3C'> 
                                    <tr>
                                       <!-- <td width='90%'><h3>"+room.IdRoom+ @"</h3></td> -->
                                        <td width='90%'><h3>STANDARD ECONOMIC ROOM</h3></td>
                                        <td><h3>" + "$"+reservation.TotalPrice.ToString()+"/"+(reservation.CheckOut-reservation.CheckIn).Days+"days"+@"</h3></td>
                                    </tr>
                                </table>
                                <table style='float: left;'>
                                    <tr height='60px'>
                                        <td><img id='icon' src='https://i.imgur.com/1wdIpUZ.png' alt='human'>"+reservation.NumberOfAdults.ToString() + "adult(s)" + (reservation.NumberOfChildren>0 ? reservation.NumberOfChildren.ToString() + "children(s)" : "")+ @"</td>
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
