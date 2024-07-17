using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Voucher
{
    public class Voucher : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        public string Description { get; set; }


    }
}
