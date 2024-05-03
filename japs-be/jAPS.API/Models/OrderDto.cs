using jAPS.API.Models.Enums;

namespace jAPS.API.Models
{
    public class OrderDto
    {
        public string DeliveryMethod { get; set; }        
        public string OrderStatus { get; set; }
    }
}