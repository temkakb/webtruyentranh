using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Viewmodels
{
    public class UploadImage_Viewmodel
    {
      public long Id { get; set; }
        public string GenreName { get; set; }
        public IList<SelectListItem> genres { get; set; }
        
        [Required(ErrorMessage = "Title cannot be blank")]
        [MaxLength(100, ErrorMessage = "Please use up to 100 characters.")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description cannot be blank")]
        [MaxLength(1000, ErrorMessage = "Please use up to 1000 characters.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Thumbnail cannot be blank")]
        public IFormFile Thumbnail { get; set; }
        [Required(ErrorMessage = "Bookcover cannot be blank")]
        public IFormFile Bookcover { get; set; }
        [Required(ErrorMessage = "Banner cannot be blank")]
        public IFormFile Banner { get; set; }
    }
}
