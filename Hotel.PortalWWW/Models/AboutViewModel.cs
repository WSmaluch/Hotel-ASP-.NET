using Hotel.Data.Data.CMS.About;
using Hotel.Data.Data.CMS.Layout;

namespace Hotel.PortalWWW.Models
{
    //public class AboutViewModel
    //{
    //    public Layout Layout { get; set; }
    //    public AboutPage AboutPage { get; set; }
    //    public IEnumerable<Hotel.Data.Data.CMS.About.AboutSilderPhoto> AboutBanner { get; set; }

    //}
    public class AboutViewModel
    {
        public AboutPage AboutUs { get; set; }
        public List<AboutSilderPhoto> AboutBanner { get; set; }
    }
}
