using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking.Extensions
{
    public class Status : AEntity
    {
        [Key]
        public int StatusId { get; set; }
        public string StatusName { get; set; }
    }
}
