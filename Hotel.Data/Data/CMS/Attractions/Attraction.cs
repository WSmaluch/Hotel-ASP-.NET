using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Attractions
{
    public class Attraction : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string MoreInfoUrl { get; set; }

        public int AttractionTypeId { get; set; }

        public AttractionType AttractionType { get; set; }

        public int AttractionPriceId { get; set; }

        public AttractionPrice AttractionPrice { get; set; }
    }
}
