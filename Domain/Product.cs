using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Net.Mime;

namespace Domain
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Naam { get; set; }
        public string Beschrijving { get; set; }
        public byte[] ImageData { get; set; }
        
        public virtual List<ProductAllergie> ProductAllergies { get; set; }

    }
}