using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
