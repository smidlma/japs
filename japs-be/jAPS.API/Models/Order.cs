namespace jAPS.API.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int TotalPrice { get; set; }
        public int PaymentMethod { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}