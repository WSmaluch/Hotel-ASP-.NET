using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking
{
    public class DiscountCode : AEntity
	{
		[Key]
		public int IdDiscountCode { get; set; }
		public string Code { get; set; }
		public decimal Discount { get; set; }
		public DateTime ValidFrom { get; set; }
		public DateTime ValidTo { get; set; }

	}
}
