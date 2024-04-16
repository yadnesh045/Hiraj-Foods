using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Checkout
    {

        [Key]
        public int id { get; set; }



        public string ProductsAndQuantity { get; set; }



        [Required]
        public string Country { get; set; }

		[Required]
		public string City { get; set; }
		[Required]
		public string Address1 { get; set; }
		[Required]
		public string Address2 { get; set; }
		[Required]
		public int pincode { get; set; }

        public decimal Total { get; set; }  
        public string paymentMethod { get; set; }



        public DateTime Date { get; set; }


        public string? PaymentStatus { get; set; }


        [ForeignKey("user")]
        public int UserId { get; set; }
        [NotMapped]
        [ValidateNever]
        public User user { get; set; }



    }
}
