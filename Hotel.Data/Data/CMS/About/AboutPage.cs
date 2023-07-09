using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.CMS.About
{
    public class AboutPage : AEntity
    {
        [Key]
        public int IdAboutPage { get; set; }
        public string BannerUrl { get; set; }
        public string BannerTitle { get; set; }
        public string Content1Title { get; set;}
        public string Content1Description { get; set;}
        public string Content1Picture1Url { get; set;}
        public string Content1Picture2Url { get; set;}
        public string Content2Title { get; set; }
        public string Content2Description { get; set; }
        public string Content3Title { get; set; }
        public string Content3Description { get; set; }
    }
}
