using System.ComponentModel.DataAnnotations;

namespace jAPS.API.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]        
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }

    }
}