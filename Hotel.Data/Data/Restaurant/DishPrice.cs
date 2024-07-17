using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Restaurant
{
    public class DishPrice : AEntity
    {
        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }
    }
}
