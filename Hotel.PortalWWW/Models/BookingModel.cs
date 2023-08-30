using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.PortalWWW.Models
{
    public class BookingModel
    {
        public int RoomId { get; set; } // Identyfikator wybranego pokoju
        public string CheckIn { get; set; } // Data zameldowania
        public string CheckOut { get; set; } // Data wymeldowania
        public int Adults { get; set; } // Liczba dorosłych gości
        public int Children { get; set; } // Liczba dzieci gości
        public IEnumerable<Facilities> facilities { get; set; } // Lista dostępnych udogodnień
        public IEnumerable<Types> types { get; set; } // Lista dostępnych typów pokoi
        public IEnumerable<Room> rooms { get; set; } // Lista dostępnych pokoi
    }
}
