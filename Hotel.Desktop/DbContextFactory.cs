using Hotel.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Desktop
{
    public class DbContextFactory
    {
        public static HotelContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<HotelContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HotelContext2023;Trusted_Connection=True;MultipleActiveResultSets=true")
                .LogTo(Console.WriteLine)
                .Options;

            return new HotelContext(options);
        }
    }
}
