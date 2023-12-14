using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking.Extensions
{
    public class Types : AEntity
    {
        [Key]
        public int IdType { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public List<Room> Rooms { get; set; }
        public string PhotosURL { get; set; } //to do typow
        //nowe
        public int Size { get; set; }
        public List<Facilities> Facilities { get; set; } = new List<Facilities>();
        public int MaxAmountOfPeople { get; set; }
        public List<RoomPricing> RoomPricings { get; set; } = new List<RoomPricing>();

        //Two-Bedroom Apartment
        //Standard Triple Room
        //Quadruple Room with Balcony
    }
}
