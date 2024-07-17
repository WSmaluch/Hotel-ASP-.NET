using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking
{
    public class Options : AEntity
    {
        [Key]
        public int IdOption { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
