using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.CMS.About
{
    public class AboutSilderPhoto : AEntity
    {
        [Key]
        public int IdAboutSilderPhoto { get; set; }

        public string PhotoUrl { get; set; }

    }
}
