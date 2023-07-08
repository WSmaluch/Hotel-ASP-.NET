using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS
{
	public class Posts : AEntity
	{
		[Key]
		public int IdPost { get; set; }

		[Display(Name = "Title")]
		[Required]
		public string Title { get; set; }

		[Display(Name = "Content")]
		[Required(ErrorMessage = "Content field is required")]
		public string Content { get; set; }

		[Display(Name = "Publish date")]
		[Required(ErrorMessage = "Publish date is required")]
		public DateTime PublishDate { get; set; }

		[Display(Name = "Author")]
		[Required]
		public string Author { get; set; }

		[Display(Name = "Photo link")]
		public string PhotoUrl { get; set; }
	}
}
