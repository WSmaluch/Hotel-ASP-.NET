using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Employess
{
    public class Salary
    {
        [Key]
        public int SalaryID { get; set; }
        public decimal SalaryAmount { get; set; }
        public string SalaryDetails { get; set; }
    }
}
