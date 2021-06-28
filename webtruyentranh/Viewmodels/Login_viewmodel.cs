using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Viewmodels
{
    public class Login_viewmodel
    {

        [Required(ErrorMessage = "Please enter your username")]
        [MaxLength(24, ErrorMessage = "Username is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Username is as least 8 charaters")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(24, ErrorMessage = "Password is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Password is as least 8 charaters")]
        public  String Password { get; set; }
    }
}
