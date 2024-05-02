using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
		[Required(ErrorMessage = "Name Is Required :")]
		public string FirstName { get; set; }
		[Required(ErrorMessage ="LastName Is Required : ")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Is Required : ")]
        [Required(ErrorMessage = "Name Is Required :")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "LastName Is Required : ")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Email Is Required : ")]

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password must be follow Below criteria : ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Phone Is Required : ")]
        [RegularExpression(@"^(\+91)?\d{10}$", ErrorMessage = "Please enter a valid 10-digit phone number.")]
        [StringLength(10, ErrorMessage = "Phone number must be exactly 10 digits.", MinimumLength = 10)]
        public string Phone { get; set; }

        public virtual UserProfileImg UserProfileImg { get; set; }
      
        public virtual ICollection<Cart> Carts { get; set; }
    }
}
