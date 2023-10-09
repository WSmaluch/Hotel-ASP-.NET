using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Booking
{
    public class ContentItem : AEntity
    {
        public string Text { get; set; }
        public string IconUrl { get; set; }
    }
}
