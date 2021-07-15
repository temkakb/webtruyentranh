using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using webtruyentranh.Viewmodels;
using webtruyentranh.Utility;
using Microsoft.AspNetCore.Http;

namespace webtruyentranh.Controllers

{
    public class ProfileController : Controller
    {
        private ComicContext _db;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;

        public ProfileController(UserManager<Account> userManager, SignInManager<Account> signInManager, ComicContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._db = db;
        }

        //profile area
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Getme()
        {
            var account = await userManager.GetUserAsync(User);

            var profile = _db.Profiles.Include(p => p.Account).ThenInclude(a=>a.Novels).Where(p => p.AccountId == account.Id).FirstOrDefault();
            ViewBag.profile = profile;
            ViewBag.isany = profile.Account.Novels.Any();
            ViewBag.isMe = true;

            return View("Getprofile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Getprofile(long Id)
        {
            Profile profile;
            try
            {
                profile = _db.Profiles.Include(p => p.Account).ThenInclude(a=>a.Novels).FirstOrDefault(p => p.Id == Id);
                ViewBag.profile = profile;
                ViewBag.isany = profile.Account.Novels.Any();
                ViewBag.isMe = false;
                return View("Getprofile");
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
        }

        [Authorize]
        [HttpGet]
       
        public async Task<IActionResult> EditProfile()
        {
            try
            {
                var profile = _db.Profiles.Include(p => p.Account).FirstOrDefault(p => p.Account.UserName == User.Identity.Name);
                var EditProfileView = new EditProfile_Viewmodel()
                {
                    Id = profile.Id,
                    DisplayName = profile.DisplayName,
                    Description = profile.Description,
                    ExternalLink = profile.ExternalLink,
                    Email = profile.Account.Email,
                    Datejoined = profile.DateJoined
                };
                ViewBag.recentavt = profile.Avartar;
                profile = null;

                return View(EditProfileView);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfile_Viewmodel editprofile, long Id)
        {
            Debug.WriteLine(editprofile.DisplayName);
            try
            {
                var profile = _db.Profiles.SingleOrDefault(p => p.Id == Id);
                if (ModelState.IsValid)
                {
                    profile.DisplayName = editprofile.DisplayName;
                    profile.Description = editprofile.Description;
                    profile.ExternalLink = editprofile.ExternalLink;
                    editprofile.Datejoined = profile.DateJoined;
                    if (editprofile.Avartar != null)
                    {
                        profile.Avartar = await Cloudinary_Utility.uploadavartar(editprofile.Avartar);
                    }
                    _db.Update(profile);
                    _db.SaveChanges();
                    ViewBag.recentavt = profile.Avartar;
                    return View(editprofile);
                }
                ModelState.AddModelError("", "can't change infomation, try again");
                return RedirectToAction("EditProfile");
            }
            catch (Exception ex)
            {
                // loi j cung tra ve 404;
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
        }

        /*---------------------------------------------------------------------------------------------------*/

        // Message area
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Message(String Content, long Id, String ReturnUrl)
        {
            try
            {
                var SenderAccount = await userManager.GetUserAsync(User);
                var ReciveAccount = userManager.Users.Include(a => a.Profile).FirstOrDefault(a => a.Id == Id);
                _db.Messages.Attach(new Message()
                {
                    Sender = SenderAccount,
                    Receiver = ReciveAccount,
                    Content = Content,
                    CreateDate = DateTime.Now
                });
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sorry, can't send message");
            }
            //if (SenderAccount.Id == Id)
            //{
            //    return RedirectToAction("Getme", "Profile");
            //}
            //else
            //    return RedirectToAction("Getprofile", "Profile",new {Id= ReciveAccount.Profile.Id });
            return Redirect(ReturnUrl);
        }

        /*---------------------------------------------------------------------------------------------------*/

        //reply area

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Reply(long Id) //get reply form

        { //get form reply for the mss
            Debug.WriteLine(Id);
            var account = await userManager.GetUserAsync(User);
            var profile = _db.Profiles.Include(p => p.Account).Where(p => p.AccountId == account.Id).FirstOrDefault();

            return ViewComponent("loadReply", new { profile = profile });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> postreply(long Id, String Content, String ReturnUrl) //post reply and return

        { //get form reply for the mss
            Message ms = _db.Messages.Where(ms => ms.Id == Id).FirstOrDefault();
            var account = await userManager.GetUserAsync(User);
            _db.ChildMessages.Attach(new ChildMessage { Account = account, Content = Content, CommentDate = DateTime.Now, Message = ms });
            _db.SaveChanges();
            return Redirect(ReturnUrl);
        }
    }
}