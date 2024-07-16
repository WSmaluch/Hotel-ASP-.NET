using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
