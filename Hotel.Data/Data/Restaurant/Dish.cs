using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Data.Data.CMS.Abstract;

namespace Hotel.Data.Data.Restaurant
{
    public class Dish : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int PriceId { get; set; }
        public DishPrice Price { get; set; }

        public int CategoryId { get; set; }
        public RestaurantCategory Category { get; set; } //Breakfast, Lunch, Dinner  

        public bool IsVegetarian { get; set; }

        public bool IsVegan { get; set; }

        public bool IsGlutenFree { get; set; }
        public string ImageUrl { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<Menu> Menu { get; set; } = new List<Menu>();
        public List<SeasonalMenu> SeasonalMenu{ get; set; } = new List<SeasonalMenu>();
        public int NutritionInfoId { get; set; }
        public NutritionInfo NutritionInfo { get; set; }
    }
}
