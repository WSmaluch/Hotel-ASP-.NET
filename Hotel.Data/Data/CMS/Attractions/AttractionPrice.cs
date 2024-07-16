using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.CMS.Attractions
{
    public class AttractionPrice : AEntity
    {
        [Key]
        public int Id { get; set; }
        public decimal Price { get; set; }
    }
}
