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


        //Two-Bedroom Apartment
        //Standard Triple Room
        //Quadruple Room with Balcony
    }
}
