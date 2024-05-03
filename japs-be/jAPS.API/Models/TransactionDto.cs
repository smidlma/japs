using jAPS.API.Models.Enums;

namespace jAPS.API.Models
{
    public class TransactionDto
    {
        public string TransactionState { get; set; }
        public string PaymentMethod { get; set; }        
        public string PaymentProvider { get; set; }
    }
}