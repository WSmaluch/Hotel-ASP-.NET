using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Contact
{
    public class ContactPage : AEntity
    {
        [Key]
        public int IdContactPage { get; set; }
        public string BannerUrl { get; set; }
        public string BannerTitle { get; set; }
        public string Localization { get; set; }
        public string AddresLine1 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}
