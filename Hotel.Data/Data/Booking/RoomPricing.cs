using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hotel.Data.Data.Booking
{
    public class RoomPricing : AEntity
    {
        [Key]
        public int PricingId { get; set; }

        [ForeignKey("Type")]
        public int TypeId { get; set; }

        public Types Type { get; set; }

        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        public decimal BasePriceAdult { get; set; }
        public decimal BasePriceChildren { get; set; }
    }
}
