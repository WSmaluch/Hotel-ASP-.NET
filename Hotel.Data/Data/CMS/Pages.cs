using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hotel.Data.Data.CMS
{
	public class Pages : AEntity
	{
		[Key]
		public int IdPage { get; set; }

		[Required]
		[MaxLength(20, ErrorMessage = "The maximum length is 20 characters")]
		[Display(Name = "Title")]
		public string Title { get; set; }

		[Required]
		[Display(Name = "Description")]
		public string Description { get; set; }



	}
}
