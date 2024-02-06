using Hotel.Intranet.Models;
using Hotel.Intranet.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Configuration;
using Hotel.Data;
using Microsoft.EntityFrameworkCore;
using Hotel.Data.Data.Booking;
using IronPdf;
using System.Runtime.Serialization;
using System.Globalization;

namespace Hotel.Intranet.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        #region Fields
        private readonly HotelContext _context;
        private decimal totalPrice;
        #endregion

        public HomeController(HotelContext context) : base(context)
        {
            //_logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            //1
            //amount of total reservations in this month
            ViewBag.Reservations = _context.Reservations
                .Where(res => res.CheckIn.Month == DateTime.Now.Month)
                .ToList();

            //amount of available rooms no matter about type
            var avaliableRooms = _context.Room
				.Where(room => !_context.Reservations
					.Any(reservation => reservation.CheckIn <= DateTime.Now.Date.AddDays(1) && reservation.CheckOut >= DateTime.Now.Date && reservation.IsActive && reservation.RoomId == room.IdRoom))
				.Count();

            var allRooms = _context.Room.Count();

			//2
			ViewBag.avaliable = avaliableRooms;

			//3
			double occupancy = (double)(allRooms - avaliableRooms) / allRooms * 100;

            ViewBag.Occupancy = Math.Floor(occupancy);

			//4
			int totalCapacity = 0;

            foreach (var type in _context.Types)
            {
                totalCapacity += type.MaxAmountOfPeople * _context.Room.Count(room => room.TypeId == type.IdType);
            }
			ViewBag.TotalCapacity = totalCapacity;

            

            ViewBag.RoomTypes = _context.Types.ToList();

			var chartData = _context.Reservations
				.Where(r => r.CheckOut >= DateTime.Now.AddMonths(-3))  // Ograniczenie do ostatnich 3 miesięcy
				.AsEnumerable()
				.GroupBy(r => new { Year = r.CheckOut.Year, Month = r.CheckOut.Month })
				.Select(g => new
				{
					CheckOut = new DateTime(g.Key.Year, g.Key.Month, 1),
					Count = g.Count()
				})
				.OrderBy(c => c.CheckOut)
				.ToList();

			ViewBag.Last3MonthsReservations = chartData;


            ViewBag.ReservationsByType = _context.Reservations
             .Where(res => res.CheckIn.Month == DateTime.Now.Month)
             .GroupBy(res => res.Room.TypeId)
             .Select(group => new
             {
                 TypeId = group.Key,
                 Count = group.Count()
             })
             .ToDictionary(item => item.TypeId, item => item.Count);



			var ddd = _context.Types.Include(t => t.Rooms).FirstOrDefault(t => t.IdType == 3);

			if (ddd != null)
			{
				int numberOfRooms = ddd.Rooms.Count;
				ViewBag.NumberOfRooms = numberOfRooms;
			}

			ViewBag.ActiveReservations = _context.Reservations
				.Where(res => res.CheckIn.Month == DateTime.Now.Month && res.IsActive)
				.ToList();

			return View();
        }

        public IActionResult AllBooking()
        {
            return View();
        }

        public IActionResult EditBooking()
        {
            return View();
        }

        public IActionResult AddBooking()
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
        public IActionResult Raport()
        {
            return View();
        }

        IEnumerable<DateTime> GetDaysBetween(DateTime start, DateTime end)
        {
            List<DateTime> days = new List<DateTime>();
            DateTime currentDay = start;

            while (currentDay <= end)
            {
                days.Add(currentDay);
                currentDay = currentDay.AddDays(1);
            }

            return days;
        }

        [HttpGet("/GenerateRaport")]
        public IActionResult GenerateRaport(int year, int month)
        {
            var query = _context.Reservations
                .Where(r => (r.CheckIn.Year == year && r.CheckIn.Month == month) || (r.CheckOut.Year == year && r.CheckOut.Month == month));

            var res = query.ToList();


            var mostCommonTypes = query
                .GroupBy(r => r.Room.TypeId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .ToList();


            var mostCommonType = mostCommonTypes?.FirstOrDefault();
            var mostCommonTypeId = mostCommonType.HasValue ? mostCommonType.Value : 0;

            var secondMostCommonType = mostCommonTypes?.Skip(1).FirstOrDefault();
            var secondMostCommonTypeId = secondMostCommonType.HasValue ? secondMostCommonType.Value : 0;




            var roomCounts = query
            .GroupBy(r => r.Room.TypeId)
            .Select(group => new
            {
                TypeId = group.Key,
                RoomCount = group.Count()
            })
            .ToList();

            var mostCommonTypeCount = roomCounts
            .OrderByDescending(x => x.RoomCount)
            .FirstOrDefault()?.RoomCount ?? 0;

            var secondMostCommonTypeCount = roomCounts
            .OrderByDescending(x => x.RoomCount)
            .Skip(1)
            .FirstOrDefault()?.RoomCount ?? 0;

            var mostCommonTypePercentage = ((mostCommonTypeCount / (double)res.Count) * 100).ToString("F2");
            var secondMostCommonTypePercentage = ((secondMostCommonTypeCount / (double)res.Count) * 100).ToString("F2");

            
            var optionCounts = query
                .Where(r => r.OptionId != null) 
                .GroupBy(r => r.OptionId)
                .Select(group => new
                {
                    OptionId = group.Key,
                    OptionCount = group.Count()
                })
                .OrderByDescending(x => x.OptionCount)
                .ToList();

            var mostCommonOptionId = optionCounts.FirstOrDefault()?.OptionId;
            var secondMostCommonOptionId = optionCounts.Skip(1).FirstOrDefault()?.OptionId ?? mostCommonOptionId;//ddadsadasdasdasdsadasdasdsa


            var mostCommonOptionCount = optionCounts
                .OrderByDescending(x => x.OptionCount)
                .FirstOrDefault()?.OptionCount ?? 0;

            var secondMostCommonOptionCount = optionCounts
                .OrderByDescending(x => x.OptionCount)
                .Skip(1)
                .FirstOrDefault()?.OptionCount ?? 0;

            
            var totalOptions = res.Count;
            var mostCommonOptionPercentage = ((mostCommonOptionCount / (double)totalOptions) * 100).ToString("F2");
            var secondMostCommonOptionPercentage = ((secondMostCommonOptionCount / (double)totalOptions) * 100).ToString("F2");

            // Dni tygodnia
            var reservationCountsByDayOfWeek = query
                .AsEnumerable()
                .SelectMany(r => GetDaysBetween(r.CheckIn.Date, r.CheckOut.Date)
                                    .Select(day => new { DayOfWeek = day.DayOfWeek, ReservationCount = 1 }))
                .GroupBy(x => x.DayOfWeek)
                .Select(group => new
                {
                    DayOfWeek = group.Key,
                    ReservationCount = group.Sum(x => x.ReservationCount) // Zmieniłem na Sum, aby policzyć łączną ilość rezerwacji danego dnia
                })
                .OrderByDescending(x => x.ReservationCount)
                .ToList();


            var totalReservations = reservationCountsByDayOfWeek.Sum(x => x.ReservationCount);

            var mostCrowdedDayOfWeek = reservationCountsByDayOfWeek.FirstOrDefault();
            var secondMostCrowdedDayOfWeek = reservationCountsByDayOfWeek.Skip(1).FirstOrDefault();
            var thirdMostCrowdedDayOfWeek = reservationCountsByDayOfWeek.Skip(2).FirstOrDefault();

            // Obliczenia procentowe
            var mostCrowdedDayOfWeekPercentage = mostCrowdedDayOfWeek != null ? Math.Round((mostCrowdedDayOfWeek.ReservationCount * 100.0 / totalReservations), 2) : 0;
            var secondMostCrowdedDayOfWeekPercentage = secondMostCrowdedDayOfWeek != null ? Math.Round((secondMostCrowdedDayOfWeek.ReservationCount * 100.0 / totalReservations), 2) : 0;
            var thirdMostCrowdedDayOfWeekPercentage = thirdMostCrowdedDayOfWeek != null ? Math.Round((thirdMostCrowdedDayOfWeek.ReservationCount * 100.0 / totalReservations), 2) : 0;
            //end 

            DateTimeFormatInfo dateinfo = new DateTimeFormatInfo();
            var totalRes = res.Count();
            double earnedMoney = 0;
            foreach (var r in res) 
            {
                earnedMoney += r.TotalPrice;
            }
            string ticketHtml = @"
        <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                <title>Monthly Raport</title>
                <style>
                    body {
                        font-family: 'Arial', sans-serif;
                        margin: 20px;
                        font-size:80%;
                    }

                    section {
                        margin-bottom: 20px;
                    }

                    h2 {
                        color: #333;
                    }

                    table {
                        border-collapse: collapse;
                        width: 100%;
                        margin-top: 10px;
                    }

                    th, td {
                        border: 1px solid #ddd;
                        padding: 8px;
                        text-align: left;
                    }

                    th {
                        background-color: #f2f2f2;
                    }

                    #logoImg
                    {
                    padding-left:42%;                    
                    width: 20%;
                    }
                    #header
                    {
                        float: left;
                    }
                </style>
            </head>
            <body>

                <img id='logoImg' src='https://i.imgur.com/EYUZVwh.png'><br/>
                <h1 >Monthly Report - "+dateinfo.GetMonthName(month)+"  "+year+@"</h1>
                <hr/>

                <section>
                    <h2 id='header'>1. General Summary</h2>
                    <table>
                        <tr>
                            <th>Monthly Profit</th>
                            <td>Total Profit: $"+earnedMoney+@"</td>
                            <td>Average Daily Profit: $"+Math.Round(earnedMoney / DateTime.DaysInMonth(year,month),2)+@"</td>
                        </tr>
                        <tr>
                            <th>Number of Reservations</th>
                            <td>Total Number of Reservations: "+totalRes+ @"</td>
                            <td>Average Daily Reservations: "+ totalRes / DateTime.DaysInMonth(year,month)+ @"</td>
                        </tr>
                    </table>
                </section>

                <section>
                    <h2>2. Room Analysis</h2>
                    <table>
                        <tr>
                            <th>Most frequently chosen room type</th>
                            <td>"+ _context.Types.Find(mostCommonTypeId).Name + @"</td>
                        </tr>
                        <tr>
                            <th>Percentage of Room Selection</th>
                            <td>"+ _context.Types.Find(mostCommonTypeId).Name + @": "+ mostCommonTypePercentage + @"%</td>
                            <td>"+ _context.Types.Find(secondMostCommonTypeId).Name + @" "+ secondMostCommonTypePercentage + @"%</td>
                        </tr>
                    </table>
                </section>

                <section>
                    <h2>3. Offer Analysis</h2>
                    <table>
                        <tr>
                            <th>Most Chosen Offers</th>
                            <td>"+ _context.Options.Find(mostCommonOptionId).Name + @" "+ mostCommonTypePercentage + @"% of reservations</td>
                            <td>"+ _context.Options.Find(secondMostCommonOptionId).Name + @" "+ secondMostCommonTypePercentage + @"% of reservations</td>
                        </tr>
                    </table>
                </section>

                <section>
                    <h2>4. Days of the Week Analysis</h2>
                    <table>
                        <tr>
                            <th>Most Crowded Days</th>
                            <td>Day of the Week: "+ mostCrowdedDayOfWeek.DayOfWeek + @"</td>
                            <td>Percentage of Reservations</td>
                            <td>"+ mostCrowdedDayOfWeek.DayOfWeek+ @": "+mostCrowdedDayOfWeekPercentage+ @"%</td>
                            <td>" + secondMostCrowdedDayOfWeek.DayOfWeek + @": " + secondMostCrowdedDayOfWeekPercentage + @"%</td>
                            <td>"+ thirdMostCrowdedDayOfWeek.DayOfWeek+ @": "+ thirdMostCrowdedDayOfWeekPercentage + @"%</td>
                        </tr>
                    </table>
                </section>

                <hr>
                The date the report was generated: " + DateTime.Now.ToShortDateString()+@"
            </body>
            </html>
            ";

            var renderer = new IronPdf.HtmlToPdf();
            renderer.PrintOptions.PaperSize = PdfPrintOptions.PdfPaperSize.A4;
            renderer.PrintOptions.MarginTop = -13;
            renderer.PrintOptions.MarginBottom = -13;
            renderer.PrintOptions.MarginLeft = -13;
            renderer.PrintOptions.MarginRight = -13;

            var pdf = renderer.RenderHtmlAsPdf(ticketHtml);

            var fileContents = pdf.BinaryData;
            return File(fileContents, "application/pdf", "Raport.pdf");
        }
    }
}