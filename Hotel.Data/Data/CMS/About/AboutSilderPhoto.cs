using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.About
{
    public class AboutSilderPhoto : AEntity
    {
        [Key]
        public int IdAboutSilderPhoto { get; set; }

        public string PhotoUrl { get; set; }

    }
}
