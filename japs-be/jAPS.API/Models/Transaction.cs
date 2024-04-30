using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;
using jAPS.API.Models.Enums;
using jAPS.API.Db;
using Microsoft.EntityFrameworkCore;

namespace jAPS.API.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        [Key]
        public Guid BasketId { get; set; }
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public PaymentProvider PaymentProvider { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public TransactionState TransactionState { get; set; }
        public int? CustomerId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]        
        public Order Order { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer? Customer { get; set; }

    }
}

