
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

    
    public class AdminController : Controller
    {
        private ComicContext _db;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;
        private readonly RoleManager<IdentityRole<long>> roleManager;

        public AdminController(UserManager<Account> userManager, SignInManager<Account> signInManager, RoleManager<IdentityRole<long>> roleManager, ComicContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._db = db;
            this.roleManager = roleManager;
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
          
            ViewData["CreatorsCount"] = _db.Accounts.Count();
            ViewData["NovelCount"] = _db.Novels.Count();
            ViewData["NovelComment"] = _db.Comments.Count();
            return View();
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public  IActionResult Finduser (String input_data)
        {
            
          
            var isNumeric = long.TryParse(input_data, out _);
            if(isNumeric)
            {
               var profile = _db.Profiles.Include(p => p.Account).Where(p=>p.Id==long.Parse(input_data)).Skip(0).Take(5).ToList();
               
                if(!profile.Any())
                {
      
                     profile = _db.Profiles.Include(p => p.Account).Where(p => p.DisplayName.Contains(input_data)).Skip(0).Take(5).ToList();
                  
                }
                return View("_userManageparticalview", profile);
            }
            else
            {
                var profile = _db.Profiles.Include(p => p.Account).Where(p => p.DisplayName.Contains(input_data)).Skip(0).Take(5).ToList();
                return View("_userManageparticalview", profile);
                
            }
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> Setadmin (long Id)
        {
            var account = _db.Accounts.Find(Id);
            if (account == null)
            {
                return Json(new { success = false,msg = "account not found" });
            }
            await userManager.AddToRoleAsync(account, "Admin");
            return Json(new { success = true, msg = $"account {account.UserName} is a admin right now!" });
        }
        [Authorize(Roles = "SuperAdmin")]
        public JsonResult Delete(long Id)
        {
            return null;
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResult block(long Id)
        {
            return null;
        }

    }
}
