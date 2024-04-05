using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class FeedBack
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
        [Display(Name = "Phone")]
        public string Phone { get; set; }


        [Required]
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; }
    }
}
