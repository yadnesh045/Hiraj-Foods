using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class Enquiry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; }

     
        [StringLength(50)]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email Is Required : ")]

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

       
        [Display(Name = "Phone")]
        [Required(ErrorMessage = "Phone Is Required : ")]
        [RegularExpression(@"^(\+91)?\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 digits.", MinimumLength = 10)]
        public string Phone { get; set; }


        [Required]
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; }


        public DateTime? Date { get; set; }




    }
}
