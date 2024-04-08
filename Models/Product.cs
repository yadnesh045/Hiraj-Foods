using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Flavour")]
        public string ProductFlavour { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Price")]
        public string ProductPrice { get; set; }


        [Required]
        [StringLength(500)]
        [Display(Name = "Nutrition")]
        public string ProductNutrition { get; set; }



        [Required]
        [StringLength(100)]
        [Display(Name = "Weight")]
        public string ProductWeight { get; set; }



        [Required]
        [StringLength(100)]
        [Display(Name = "Ingredients")]
        public string ProductIngredient { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }

        [Required]
        [NotMapped]
        [Display(Name = "FlavourImage")]
        public IFormFile ProductFlavourImage { get; set; }

        public string? ProductFlavourImageUrl { get; set; }


        [Required]
        [NotMapped]
        [Display(Name = "ProductImage")]
        public IFormFile ProductImage { get; set; }

        public string? ProductImageUrl { get; set; }
    }
}
