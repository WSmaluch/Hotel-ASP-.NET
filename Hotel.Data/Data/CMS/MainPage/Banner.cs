using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.MainPage
{
    public class Banner : AEntity
    {
        [Key]
        public int IdBanner { get; set; }

        public string BannerTitle { get; set; }
        public string BannerDescription { get; set; }
        public string BannerUrl { get; set; }

    }
}

