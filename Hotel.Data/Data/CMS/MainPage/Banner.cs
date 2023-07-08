using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

