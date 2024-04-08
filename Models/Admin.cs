using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Admin
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        //[Required]
        //[NotMapped]
        //[Display(Name = "Image")]
        //public IFormFile Image { get; set; }


        //public string ImageUrl { get; set; }

        [NotMapped]
        [StringLength(50)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set;}


        [Required]
        [StringLength(50)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }


    }
}
