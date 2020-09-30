using System.Collections.Generic;
using Domain;

namespace Winkeltje.Models
{
    public class ProductIndexModel
    {
        public IList<Allergie> Allergies { get; set; }
        public IList<Product> Products { get; set; }
        public string SearchString { get; set; }
        public IList<WinkelItem> WinkelItems { get; set; }
    }
}
