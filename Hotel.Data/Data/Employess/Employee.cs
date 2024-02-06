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
        public ICollection<Contact> Contacts { get; set; }
        public ICollection<Department> Departments { get; set; }
        public ICollection<Qualification> Qualifications { get; set; }
        public ICollection<Salary> Salaries { get; set; }
    }
}
