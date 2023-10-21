using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking
{
    public class Options : AEntity
    {
        [Key]
        public int IdOption { get; set; }
        public string Name { get; set; }
        public string PhotoUrl { get; set; }
        public List<ContentItem> ContentItems { get; set; } = new List<ContentItem>();
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
