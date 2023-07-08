using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hotel.Data.Data.CMS.Abstract
{
	abstract public class AEntity
	{
		[Display(Name = "Is active?")]
		public bool IsActive { get; set; } = true;

		[Display(Name = "Added by")]
		public string? AddedBy { get; set; }

		[Display(Name = "Added date")]
		[Required(ErrorMessage = "Date added field is required")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime AddedDate { get; set; } = DateTime.Now;

		[Display(Name = "Modified by")]
		public string? ModifiedBy { get; set; }

		[Display(Name = "Modified date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? ModifiedDate { get; set; }

		[Display(Name = "RemovedBy")]
		public string? RemovedBy { get; set; }

		[Display(Name = "Removed date")]
		[DataType(DataType.DateTime)]
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
		public DateTime? RemovedDate { get; set; }
	}
}
