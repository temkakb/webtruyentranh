using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Viewmodels
{
    public class EditProfile_Viewmodel
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Display name cannot be blank")]
        [MaxLength(20, ErrorMessage = "Please use up to 20 characters.")]
        public string DisplayName { get; set; }

        [MaxLength(255, ErrorMessage = "Please use up to 255 characters.")]
        public string Description { get; set; }

        public IFormFile Avartar { get; set; }
        [MaxLength(512, ErrorMessage = "Url length is to long")]
        public string ExternalLink { get; set; }
        public String Email { get; set; }
        public DateTime Datejoined { get; set; }
    }
}
