using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GetStartAspNet.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetadata
    {
        [Display(Name ="Fist Name")]
        [Required(AllowEmptyStrings =false, ErrorMessage = "First name required")]
        public string FistName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Email Id")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email Id  required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [MinLength(6,ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPassword required")]
        public string ConfirmPassword { get; set; }
        
    }
}