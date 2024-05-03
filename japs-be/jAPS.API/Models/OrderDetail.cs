namespace jAPS.API.Models
{
    public class OrderDetail
    {
        public int OrderId { get; set; }
        public Basket Basket { get; set; }
        public Customer Customer { get; set; }
        public OrderDto OrderDto { get; set; }
        public TransactionDto TransactionDto { get; set; }
        
    }
}