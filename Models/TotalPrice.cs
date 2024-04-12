using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class TotalPrice
    {

        [Key]
        public int Id { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("user")]
        public int UserId { get; set; }
        [NotMapped]
        [ValidateNever]
        public User user { get; set; }


    }
}
