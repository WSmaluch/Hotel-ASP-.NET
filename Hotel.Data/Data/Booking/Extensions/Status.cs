using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Booking.Extensions
{
    // Represents the status of a rooms and others
    public class Status : AEntity
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
