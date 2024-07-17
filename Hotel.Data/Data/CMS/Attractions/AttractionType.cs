using Hotel.Data.Data.CMS.Abstract;
using System.ComponentModel.DataAnnotations;

namespace Hotel.Data.Data.CMS.Attractions
{
    public class AttractionType : AEntity
    {
        [Key]
        public int AttractionTypeId { get; set; }

        public string Name { get; set; }

    }
}
