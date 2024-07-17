using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Hotel.Data.Data.Restaurant
{
    public class Ingredient : AEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Dish> Dishes { get; set; } = new List<Dish>();

    }
}
