
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace GetStartAspNet.Models
{
 
    public partial class User
    {
        [Display(Name = "Fist Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "First name required")]
        public string FistName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name  required")]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPassword required")]
        public string ConfirmPassword { get; set; }

    }


}