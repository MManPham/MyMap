using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GetStartAspNet.Models
{
    public class UserLogin
    {
        [Display(Name = "User Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "User Name required")]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password required")]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string Password { get; set; }

        [Display(Name = "Remeber Me")]
        public  bool RememberMe { get; set; }
    }
}