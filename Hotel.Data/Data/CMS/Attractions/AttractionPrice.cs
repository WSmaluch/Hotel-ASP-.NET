using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Attractions
{
    public class AttractionPrice : AEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
