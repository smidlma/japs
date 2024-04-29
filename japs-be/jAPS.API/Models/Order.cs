using jAPS.API.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace jAPS.API.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public DeliveryMethod DeliveryMethod { get; set; }
        [Required]
        public OrderStatus OrderStatus { get; set; }        
        public List<OrderItem> OrderItems { get; set; }
        public Transaction Transaction { get; set; }       
        
    }
}