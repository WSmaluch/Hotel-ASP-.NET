using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.CMS.Attractions
{
    public class Attraction : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string MoreInfoUrl { get; set; }

        public int AttractionTypeId { get; set; }

        public AttractionType AttractionType { get; set; }

        public int AttractionPriceId { get; set; }

        public AttractionPrice AttractionPrice { get; set; }
    }
}
