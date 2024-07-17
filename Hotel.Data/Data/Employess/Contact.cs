using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Employess
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

    }
}
