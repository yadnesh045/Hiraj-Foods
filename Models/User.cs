using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
		[Required(ErrorMessage ="Name Is Required :")]
		public string FirstName { get; set; }
		[Required(ErrorMessage ="LastName Is Required : ")]
		public string LastName { get; set; }
		[Required(ErrorMessage = "Email Is Required : ")]
		//[RegularExpression(@"\A[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}\z", ErrorMessage = "Please enter a valid email address")]
		[EmailAddress(ErrorMessage = "Invalid email address")]
		public string Email { get; set; }
		
		[Required(ErrorMessage = "Password must be follow Below criteria : ")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$")]
		public string Password { get; set; }
		[Required(ErrorMessage = "Phone Is Required : ")]
		public string Phone { get; set; }
       

    }
}
