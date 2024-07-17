using Hotel.Data.Data.Booking;
using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.Employess;

namespace Hotel.Data.Data.Desktop
{
    public class RepairTask
    {
        public int Id { get; set; }
        public DateTime ScheduledDate { get; set; }
        public int RoomId { get; set; } // Klucz obcy
        public int? EmployeeId { get; set; } // Klucz obcy do pracownika 
        public string Note { get; set; }

        public Room Room { get; set; } // Relacja do pokoju
        public Employee Employee { get; set; } // Relacja do pracownika
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }

        public Status? RepairStatus { get; set; }
    }

}
