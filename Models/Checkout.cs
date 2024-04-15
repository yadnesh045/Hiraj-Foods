using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Checkout
    {

        [Key]
        public int id { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Address1 { get; set; }

        public string Address2 { get; set; }

        public int pincode { get; set; }

        public decimal Total { get; set; }  
        public string paymentMethod { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }
        [NotMapped]
        [ValidateNever]
        public User user { get; set; }



    }
}
