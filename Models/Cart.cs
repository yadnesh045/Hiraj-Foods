using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }


        [ForeignKey("User")]
        public int UserId { get; set; }


        [NotMapped]
        [ValidateNever]
        public User User { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }


        [NotMapped]
        [ValidateNever]
        public Product Product { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public int Quantity { get; set; }



        public string ProductWeight { get; set; }

        public string ProductImageUrl { get; set; }

        public string ProductPrice { get; set; }

    }
}
