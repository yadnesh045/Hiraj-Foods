﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hiraj_Foods.Models
{
    public class Admin
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }



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
        [Display(Name = "ImageUrl")]
        public string? ImageUrl { get; set; }

        [NotMapped]
        [StringLength(50)]
        [Display(Name = "NewPassword")]
        public string NewPassword { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "Mobile")]
        public string Mobile { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Address")]
        public string Address { get; set; }


        [StringLength(100)]
        [Display(Name = "State")]
        public string? State { get; set; }


        
        [Display(Name = "ZipCode")]
        [StringLength(6)]
        public string? ZipCode { get; set; }


        [Display(Name = "IsActive")]
        public bool? IsActive { get; set; }






    }
}
