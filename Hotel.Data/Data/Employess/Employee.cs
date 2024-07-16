using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Employess
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime HiringDate { get; set; }

        public int ContactId { get; set; }
        public Contact Contact { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int QualificationId { get; set; }
        public Qualification Qualification { get; set; }

        public int SalaryId { get; set; }
        public Salary Salary { get; set; }
    }
}
