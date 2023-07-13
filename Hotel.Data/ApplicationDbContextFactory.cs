using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<HotelContext>
    {
        public HotelContext CreateDbContext(string[] args) { var optionsBuilder = new DbContextOptionsBuilder<HotelContext>(); optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=HotelContext2023;Trusted_Connection=True;MultipleActiveResultSets=true"); return new HotelContext(optionsBuilder.Options); }
    }
}
