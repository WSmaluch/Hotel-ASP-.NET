using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
