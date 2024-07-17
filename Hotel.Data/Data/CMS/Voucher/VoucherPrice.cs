using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Voucher
{
    public class VoucherPrice : AEntity
    {
        [Key]
        public int Id { get; set; }  

        public decimal Amount { get; set; }
        public DateTime ValidFrom { get; set; }

        public DateTime ValidTo { get; set; }
    }
}
