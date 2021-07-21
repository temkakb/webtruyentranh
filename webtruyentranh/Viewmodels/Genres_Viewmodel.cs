using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Viewmodels
{
    public class Genres_Viewmodel
    {
        [Key]
        public long Id { get; set; }
        public string GenreName { get; set; }
         
  
    
        public IList<SelectListItem>  genres { get; set; }
    }
}
