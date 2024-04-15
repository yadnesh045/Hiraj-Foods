using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class UserProfileImg
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        [ValidateNever]
        public int UserId { get; set; }


        [NotMapped]
        [ValidateNever]
        public User User { get; set; }


        [ValidateNever]
        public string user_Profile_Img { get; set; }
    }
}
