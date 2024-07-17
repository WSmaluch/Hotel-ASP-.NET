using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Employess
{
    public class Qualification
    {
        [Key]
        public int QualificationID { get; set; }
        public string QualificationName { get; set; }
    }
}
