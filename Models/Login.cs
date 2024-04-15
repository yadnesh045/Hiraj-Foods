using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class Login
    {

		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
    }
}
