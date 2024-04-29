namespace jAPS.API.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public Product? Product { get; set; }
    }
}
