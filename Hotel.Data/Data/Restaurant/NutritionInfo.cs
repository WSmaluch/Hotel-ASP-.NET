using Hotel.Data.Data.CMS.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Data.Data.Restaurant
{
    public class NutritionInfo : AEntity
    {
        [Key]
        public int Id { get; set; }

        public decimal Calories { get; set; }

        public decimal Protein { get; set; }

        public decimal Carbohydrates { get; set; }

        public decimal Fat { get; set; }

    }
}
