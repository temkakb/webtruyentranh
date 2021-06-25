using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;

    public class AuthenticationController : Controller
    {
        private ComicContext db;


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {

            return View();
            
            
        }
    }

