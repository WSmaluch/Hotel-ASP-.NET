using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking.Extensions
{
    public class Facilities : AEntity
    {
        [Key]
        public int IdFacility { get; set; }
        public string NameFacility { get; set; }

        //        TV
        //Linen
        //Dining table
        //Carpeted
        //Flat-screen TV
        //Electric kettle
    }
}
