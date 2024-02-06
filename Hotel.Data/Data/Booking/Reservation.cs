using Hotel.Data.Data.Booking.Extensions;
using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking
{
    public class Reservation : AEntity
    {
        [Key]
        public int IdReservation { get; set; }
        public int RoomId { get; set; }
        public Room Room { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string AdressFirstLine { get; set; }
        public string PostalCode { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int NumberOfAdults { get; set; }
        public int NumberOfChildren { get; set; }
        public string? SpecialRequests { get; set; }
        public double TotalPrice { get; set; }
        public int OptionId { get; set; }
        public Options Option { get; set; } 
        public int? StatusId { get; set; }
        public Status? ReservationStatus { get; set; }
    }
}
