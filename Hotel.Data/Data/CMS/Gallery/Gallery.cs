using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Gallery
{
    public class Gallery : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }
    }
}
