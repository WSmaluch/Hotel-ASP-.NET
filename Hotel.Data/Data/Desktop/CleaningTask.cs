using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
