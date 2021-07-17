
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
        /*------------------------------------------------------manage USER-------------------------------------------------------*/
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
            try
            {
                await userManager.RemoveFromRoleAsync(account, "Member");
                await userManager.AddToRoleAsync(account, "Admin");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"account {account.UserName} is already admin " });
            }
           
            return Json(new { success = true, msg = $"account {account.UserName} is a admin right now!" });
        }
        [HttpGet]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<JsonResult> Setuser(long Id)
        {
            var account = _db.Accounts.Find(Id);
            if (account == null)
            {
                return Json(new { success = false, msg = "account not found" });
            }
            try
            {
                await userManager.RemoveFromRoleAsync(account, "Admin");
                await userManager.AddToRoleAsync(account, "Member");
            }
            catch (Exception ex)
            {
                return Json(new { success = false, msg = $"account {account.UserName} is already member " });
            }

            return Json(new { success = true, msg = $"account {account.UserName} is a member right now!" });
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResult> Blockuser(long Id)
        {

            var account = _db.Accounts.Find(Id);
            
            if (account == null)
            {
                return Json(new { success = false, msg = "account not found" });
            }
            if (await userManager.IsInRoleAsync(account, "Admin"))
            {
                return Json(new { success = false, msg = "This user is admin, cannot block" });
            }
            if (await userManager.IsLockedOutAsync(account))
            {
                return Json(new { success = false, msg = $"Account {account.UserName} account has been blocked for 200 years!! " });
            }
            await userManager.SetLockoutEndDateAsync(account, DateTime.Today.AddYears(200)); // set 200 nam mo khoa nhe cu
            return Json(new { success = true, msg = $"Account {account.UserName} has been blocked" });
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<JsonResult> Unblockuser(long Id)
        {
            var account = _db.Accounts.Find(Id);
            if (account == null)
            {
                return Json(new { success = false, msg = "account not found" });
            }
            await userManager.SetLockoutEndDateAsync(account, null); // set 200 nam mo khoa nhe cu
            return Json(new { success = true, msg = $"Account {account.UserName} has been unblock" });
        }

        /*------------------------------------------------------manage NOVEL-------------------------------------------------------*/
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Findnovel(String input_data)
        {
            var novel = _db.Novels.Include(n=>n.Account).Include(n=>n.Episodes).Where(n => n.Title.Contains(input_data)).Skip(0).Take(5).ToList();
            return View("_novelsManageparticalview", novel);
    
        }
        [Authorize(Roles = "Admin,SuperAdmin")]
        public JsonResult Removenovel(long Id)
        {

            var novel = _db.Novels.Include(n => n.Episodes).ThenInclude(e => e.Comments).ThenInclude(cm => cm.ChildComments).Where(n => n.Id == Id)
                   .FirstOrDefault();
            if (novel != null)
            {
                _db.Remove(novel);
                _db.SaveChanges();
                return Json(new { success = true, msg = $"Novel {novel.Title} has been removed" });
            }
            else
            {
                return Json(new { success = true, msg = $"Novel {novel.Title} not found" });
            }
            return Json(new { success = false, msg = "Error while handling" });


        }
        /*------------------------------------------------------manage Genre-------------------------------------------------------*/
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult LoadGenrecontent()
        {
            var g = _db.Genres.ToList();
            return PartialView("GenreManageparicalview",g);
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult Loadform(long Id)
        {
       
            ViewBag.Titlemodal = "Update Genre Name";
            ViewBag.Id = Id;
            return PartialView("LoadmodalUpdate");
        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult updateGenre(long Id , String name)
        {
            try
            {
                var genre = _db.Genres.Find(Id);
                genre.GenreName = name;
                _db.Update(genre);
                _db.SaveChanges();
                return Json(new { success = true, msg = $"genre has been change to {name}" });
            }
            catch(Exception ex)
            {
                return Json(new { success = true, msg = "Some error while handling" });
            }
        }
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult LoadformnewGenre()
        {

            ViewBag.Titlemodal = "Create new genre ";
        
            return PartialView("LoadmodalCreate");
        }
        [Authorize(Roles = "SuperAdmin")]
        [ValidateAntiForgeryToken]
        [HttpPost]

        public JsonResult CreateGenre(String name)
        {
            
            _db.Genres.Add(new Genre { GenreName = name });
            _db.SaveChanges();

            return Json(new { success = true, msg = "Successed" });
        }
    }


}

