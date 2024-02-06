using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Employess
{
    public class Qualification
    {
        [Key]
        public int QualificationID { get; set; }
        public int EmployeeID { get; set; }
        public string HeldQualifications { get; set; }
        public Employee Employee { get; set; }
    }
}
