using Hotel.Data.Data.CMS;
using Hotel.Data.Data.CMS.MainPage;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Data
{
	public class HotelContext : DbContext
	{
		public HotelContext(DbContextOptions<HotelContext> options)
			: base(options)
		{
		}

		public DbSet<Pages> Pages { get; set; } = default!;

		public DbSet<Posts>? Posts { get; set; }
		public DbSet<Banner>? Banner { get; set; }
		public DbSet<Video>? Video { get; set; }
	}
}
