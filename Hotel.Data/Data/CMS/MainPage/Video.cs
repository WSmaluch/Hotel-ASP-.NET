using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.MainPage
{
    public class Video : AEntity
    {
        [Key]
        public int IdVideo { get; set; }
        public string VideoUrl { get; set; } = string.Empty;
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
    }
}
