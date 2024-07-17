using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS;
using Hotel.Data.Data.CMS.About;
using Hotel.Data.Data.CMS.Attractions;
using Hotel.Data.Data.CMS.Blog;
using Hotel.Data.Data.CMS.Contact;
using Hotel.Data.Data.CMS.Gallery;
using Hotel.Data.Data.CMS.Layout;
using Hotel.Data.Data.CMS.MainPage;
using Hotel.Data.Data.CMS.Newsletter;
using Hotel.Data.Data.CMS.Offers;
using Hotel.Data.Data.CMS.Site_Guid;
using Hotel.Data.Data.CMS.Voucher;
using Hotel.Data.Data.Desktop;
using Hotel.Data.Data.Employess;
using Hotel.Data.Data.Restaurant;
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
		public DbSet<Cleaner>? Cleaner { get; set; }
		public DbSet<CleaningTask>? CleaningTask { get; set; }
		public DbSet<RepairTask>? RepairTask { get; set; }
		public DbSet<Employee>? Employee { get; set; }
		public DbSet<Contact>? Contact { get; set; }
		public DbSet<Department>? Department { get; set; }
		public DbSet<Qualification>? Qualification { get; set; }
		public DbSet<Salary>? Salary { get; set; }
		public DbSet<Status>? Status { get; set; }
		public DbSet<DiscountCode>? DiscountCode { get; set; }
		public DbSet<Newsletter>? Newsletter { get; set; }
		public DbSet<NewsletterConfig>? NewsletterConfig { get; set; }
		public DbSet<Attraction>? Attraction { get; set; }
		public DbSet<AttractionType>? AttractionType { get; set; }
		public DbSet<AttractionPrice>? AttractionPrice { get; set; }
		public DbSet<Voucher>? Vouchers { get; set; }
		public DbSet<VoucherPrice>? VoucherPrice { get; set; }
		public DbSet<VoucherType>? VoucherType { get; set; }
		public DbSet<SiteGuide>? SiteGuide { get; set; }
		public DbSet<Gallery>? Gallery { get; set; }
        //restaurant
        public DbSet<RestaurantPage>? RestaurantPage { get; set; }
        public DbSet<CulinaryEvent>? CulinaryEvent { get; set; }
        public DbSet<Dish>? Dish { get; set; }
        public DbSet<DishPrice>? DishPrice { get; set; }
        public DbSet<Ingredient>? Ingredient { get; set; }
        public DbSet<Menu>? Menu { get; set; }
        public DbSet<NutritionInfo>? NutritionInfo { get; set; }
        public DbSet<Promotion>? Promotion { get; set; }
        public DbSet<RestaurantCategory>? RestaurantCategory { get; set; }
        public DbSet<RestaurantSchedule>? RestaurantSchedule { get; set; }
        public DbSet<SeasonalMenu>? SeasonalMenu { get; set; }
    }
}
