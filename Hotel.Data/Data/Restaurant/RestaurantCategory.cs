using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.Restaurant
{
    public class RestaurantCategory : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }

}
