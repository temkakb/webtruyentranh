using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace webtruyentranh.Viewmodels
{
    public class Register_viewmodel
    {

        [Required(ErrorMessage = "Please enter your username")]
        [MaxLength(24, ErrorMessage = "Username is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Username is as least 8 charaters")]
        public String R_UserName { get; set; }

        [Required(ErrorMessage = "Please enter your email ")]
        [EmailAddress(ErrorMessage = "Email is not valid")]

        public String R_Email { get; set; }

        [Required(ErrorMessage = "Please enter your password")]

        [MaxLength(24, ErrorMessage = "Password is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Password is as least 8 charaters")]
        
        public String R_Password { get; set; }

        [Required(ErrorMessage = "Please comfirm your password")]
    
        [Compare("R_Password",ErrorMessage = "Password did not match")]

        public String R_Password2 { get; set; }
    
    }
}
