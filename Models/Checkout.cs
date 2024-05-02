using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Checkout
    {

        [Key]
        public int id { get; set; }


        [ValidateNever]
        public string? ProductsAndQuantity { get; set; }



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


        [ValidateNever]
        public decimal Total { get; set; }
        [ValidateNever]
        public string paymentMethod { get; set; }


        [ValidateNever]
        public DateTime Date { get; set; }

        [ValidateNever]
        public string? PaymentStatus { get; set; }

        [Required]
        public string TranscationID { get; set; }


        [ForeignKey("user")]
        public int UserId { get; set; }
        [NotMapped]
        [ValidateNever]
        public User user { get; set; }



    }
}
