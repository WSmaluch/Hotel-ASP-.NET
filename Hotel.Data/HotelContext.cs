﻿using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS;
using Hotel.Data.Data.CMS.About;
using Hotel.Data.Data.CMS.Blog;
using Hotel.Data.Data.CMS.Contact;
using Hotel.Data.Data.CMS.Layout;
using Hotel.Data.Data.CMS.MainPage;
using Hotel.Data.Data.CMS.Offers;
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
		public DbSet<Layout>? Layout { get; set; }
		public DbSet<Posts>? Posts { get; set; }
		public DbSet<Banner>? Banner { get; set; }
		public DbSet<Video>? Video { get; set; }
		public DbSet<AboutPage>? AboutPage { get; set; }
		public DbSet<AboutSilderPhoto>? AboutSilderPhoto { get; set; }
		public DbSet<ContactPage>? ContactPage { get; set; }
		public DbSet<Post>? Post { get; set; }
		public DbSet<Offer>? Offer { get; set; }
		public DbSet<Facilities>? Facilities { get; set; }
		public DbSet<Types>? Types { get; set; }
		public DbSet<Room>? Room { get; set; }
		public DbSet<Reservation>? Reservations { get; set; }
		public DbSet<Options>? Options { get; set; }
		public DbSet<ContentItem>? ContentItem { get; set; }
		public DbSet<RoomPricing>? RoomPricing { get; set; }
    }
}
