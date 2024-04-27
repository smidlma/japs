namespace jAPS.API.Models
{
    public class Transaction
    {
        public Guid BasketId { get; set; }
        public int PaymentMethod  { get; set; }       
        public int CustomerId { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public Customer Customer { get; set; }        

    }
}
