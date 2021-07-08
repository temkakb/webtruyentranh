
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace webtruyentranh.Controllers
{

    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ComicContext _db;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;

        public AdminController(UserManager<Account> userManager, SignInManager<Account> signInManager, ComicContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._db = db;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Finduser (String username)
        {
          
                Debug.WriteLine(username);
                var profile = _db.Profiles.Include(p => p.Account).Where(p => p.DisplayName.Contains(username));
         
            return View("_userManageparticalview",profile);
        }
        [HttpPost]
        public JsonResult Setadmin (long Id)
        {
            return null;
        }
        public JsonResult Delete(long Id)
        {
            return null;
        }
        public JsonResult block(long Id)
        {
            return null;
        }

    }
}
