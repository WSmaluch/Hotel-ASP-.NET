using System.ComponentModel.DataAnnotations;

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
