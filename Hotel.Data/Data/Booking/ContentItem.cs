using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking
{
    public class ContentItem : AEntity
    {
        [Key]
        public int IdContentItem { get; set; }
        public string Text { get; set; }
        public string IconUrl { get; set; }
        [JsonIgnore]
        public List<Options> Options { get; set; } = new List<Options>();
    }
}
