using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking
{
    public class Room : AEntity
    {
        [Key]
        public int IdRoom { get; set; }
        public int RoomNumber { get; set; }
        public decimal Price { get; set; }
        public string PhotosURL { get; set; }
        public int TypeId { get; set; }
        public Types Type { get; set; }

    }
}
