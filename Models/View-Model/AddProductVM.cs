using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Controllers
{
    public class AddProductVM
    {

        public string ProductName { get; set; }

        public string ProductFlavour { get; set; }


        public string ProductPrice { get; set; }


      
        public string ProductNutrition { get; set; }


        public string ProductWeight { get; set; }


        public string ProductIngredient { get; set; }

        public string ProductDescription { get; set; }

        public IFormFile ProductFlavourImage { get; set; }

        public string? ProductFlavourImageUrl { get; set; }

        public IFormFile ProductImage { get; set; }

        public string? ProductImageUrl { get; set; }

    }
}