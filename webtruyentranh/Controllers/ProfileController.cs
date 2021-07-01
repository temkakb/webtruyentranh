using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Controllers
{
   
    public class ProfileController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Getme()
        {
            return View("Getprofile");
        }
        public IActionResult Getprofile(long Id)
        {
            return null;
        }
       
    }
}
