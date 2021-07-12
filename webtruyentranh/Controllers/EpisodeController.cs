using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webtruyentranh.Controllers
{
    public class EpisodeController : Controller
    {
        public IActionResult getlistepisode()
        {
            
            return View();
        }

        public IActionResult Episode()
        {

            return View();
        }

    }
}
