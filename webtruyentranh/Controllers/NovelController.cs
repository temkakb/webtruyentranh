using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;

namespace webtruyentranh.Controllers
{
    

    public class NovelController : Controller
    {   
        public IActionResult Index()
        {
            return View();
        }
    }
}
