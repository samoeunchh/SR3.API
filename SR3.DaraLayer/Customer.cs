using System;
using System.ComponentModel.DataAnnotations;

namespace SR3.DaraLayer
{
    public class Customer
    {
        [Key]
        public Guid CustomerId { get; set; }
        [Required(ErrorMessage = "Customer name is required")]
        [MaxLength(50)]
        public string CustomerName { get; set; }
        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Mobile { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
    }
}
