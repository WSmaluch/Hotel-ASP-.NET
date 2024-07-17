using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS.Abstract;
using Hotel.Data.Data.Desktop;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking
{
    public class Room : AEntity
    {
        [Key]
        public int IdRoom { get; set; } 
        public int TypeId { get; set; } 
        public int Number { get; set; } 
        public Types Type { get; set; } 
        public List<Facilities?> Facilities { get; set; } = new List<Facilities>(); 
        public ICollection<CleaningTask> CleaningTasks { get; set; }
        public int? StatusId { get; set; }
        public Status? RoomStatus { get; set; }

    }
}
