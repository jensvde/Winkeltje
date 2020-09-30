using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Allergie
    {
        [Key]
        public int AllergieId { get; set;  }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public virtual List<ProductAllergie> ProductAllergies { get; set; }
    }
}