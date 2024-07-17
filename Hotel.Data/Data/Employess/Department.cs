using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Employess
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
}
