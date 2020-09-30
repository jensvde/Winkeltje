namespace Domain
{
    public class ProductAllergie
    {
        public int ProductId { get; set; }
        public int AllergieId { get; set; }
        public Allergie Allergie { get; set;  }
        public Product Product { get; set; }
    }
}