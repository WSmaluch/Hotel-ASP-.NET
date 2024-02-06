using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS.Abstract;
using Hotel.Data.Data.Desktop;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking
{
    public class Room : AEntity
    {
        [Key]
        public int IdRoom { get; set; } //to zostaje
        public int TypeId { get; set; } //to zostaje
        public int Number { get; set; } //to zostaje
        public Types Type { get; set; } //to zostaje
        public List<Facilities?> Facilities { get; set; } = new List<Facilities>(); //to zostaje, bo to moga byc dodatkowe (tylko dla tego pokoju)
        public ICollection<CleaningTask> CleaningTasks { get; set; }
        public int? StatusId { get; set; }
        public Status? RoomStatus { get; set; }

    }
}
