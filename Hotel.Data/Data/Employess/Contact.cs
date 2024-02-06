using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Employess
{
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }

        // Foreign key
        public int EmployeeID { get; set; }

        // Contact properties
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        // Navigation property
        public Employee Employee { get; set; }
    }
}
