using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Orders
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }


        [NotMapped]
        [ValidateNever]
        public User User { get; set; }


        [Required]
        [Display(Name = "Product Name")]
        public string Products { get; set; }


        public decimal Total { get; set; }

        [ValidateNever]
        public DateTime date { get; set; }

        public string Paymentmethod { get;  set; }

        public string status { get; set; }
    }
}
