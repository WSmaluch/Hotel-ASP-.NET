using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Newsletter
{
    public class Newsletter : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Email { get; set; }

        public DateTime SubscribedOn { get; set; }
    }
}
