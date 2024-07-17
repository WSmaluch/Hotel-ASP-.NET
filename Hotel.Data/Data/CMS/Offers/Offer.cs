using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Offers
{
    public class Offer : AEntity
    {
        [Key]
        public int IdOffer { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public string PhotoUrl { get; set; }
    }
}
