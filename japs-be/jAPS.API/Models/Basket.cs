namespace jAPS.API.Models
{
    public class Basket
    {
        public Guid? BasketId { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ProductDto> Items { get; set; }
    }
}