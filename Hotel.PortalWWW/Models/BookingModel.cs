using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.PortalWWW.Models
{
    public class BookingModel
    {
        public int RoomId { get; set; } // Identifier of the selected room
        public string CheckIn { get; set; } // Check-in date
        public string CheckOut { get; set; } // Check-out date
        public int Adults { get; set; } // Number of adult guests
        public int Children { get; set; } // Number of child guests
        public IEnumerable<Facilities> facilities { get; set; } // List of available facilities
        public IEnumerable<Types> types { get; set; } // List of available room types
        public IEnumerable<Room> rooms { get; set; } // List of available rooms
        public List<Options> Options { get; set; }
        public Dictionary<int, decimal> PricesByRoomType { get; set; }
    }
}
