using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hotel.Data.Data.Booking.Extensions
{
    // Represents the Facilities entity
    public class Facilities : AEntity
    {
        [Key]
        public int IdFacility { get; set; } // Unique identifier for the facility
        public string NameFacility { get; set; } // Name of the facility
        [JsonIgnore]
        public List<Room> Rooms { get; set; } = new List<Room>(); // List of rooms associated with the facility
        public List<Types> Types { get; set; } = new List<Types>(); // List of types associated with the facility
    }
}
