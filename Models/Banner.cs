using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class Banner
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string Flavour_title { get; set; }
        [ValidateNever]

        public string Banner_Img { get; set; }


    }
}
