using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;

namespace Hotel.Data.Data.Desktop
{
    public class CleaningTask
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int RoomId { get; set; } // Klucz obcy
        public Room Room { get; set; } // Relacja do pokoju
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public Status? ReservationStatus { get; set; }
    }
}
