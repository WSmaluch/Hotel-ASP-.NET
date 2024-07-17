using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking.Extensions
{
    // Represents the types of rooms available
    public class Types : AEntity
    {
        [Key]
        public int IdType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Room> Rooms { get; set; }
        public string PhotosURL { get; set; }
        public int Size { get; set; }
        public List<Facilities> Facilities { get; set; } = new List<Facilities>();
        public int MaxAmountOfPeople { get; set; }
        public List<RoomPricing> RoomPricings { get; set; } = new List<RoomPricing>();
    }
}
