using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;
using Microsoft.EntityFrameworkCore;

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
            Debug.WriteLine(account.Id);
            var profile = _db.Profiles.Include(p=>p.Account).Where(p => p.AccountId == account.Id).FirstOrDefault();
            Debug.WriteLine(profile.DisplayName);
            Debug.WriteLine(profile.Avartar);
            var novels = _db.Novels.Where(n => n.Account.Id == account.Id).ToList();
            ViewBag.profile = profile;
            ViewBag.novels = novels;
            ViewBag.isany = novels.Any();
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
                profile = _db.Profiles.Include(p => p.Account).FirstOrDefault(p => p.Id == Id);
                var novels = _db.Novels.Where(n => n.Account.Id == profile.Account.Id).ToList();
                ViewBag.profile = profile;
                ViewBag.novels = novels;
                ViewBag.isany = novels.Any();
                ViewBag.isMe = false;

                return View("Getprofile");
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
    
           
        }

        // Message area
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Message(String Content, long Id ,String ReturnUrl)
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

        //reply area

        [Authorize]
        [HttpGet]
        public async Task< IActionResult> Reply(long Id) //get reply form

        { //get form reply for the mss
            Debug.WriteLine(Id);
            var account = await userManager.GetUserAsync(User);
            var profile = _db.Profiles.Include(p => p.Account).Where(p => p.AccountId == account.Id).FirstOrDefault();

            return ViewComponent("loadReply",new { profile = profile, Id = Id });
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> postreply (long Id, String Content, String ReturnUrl) //post reply and return

        { //get form reply for the mss
         
            Message ms = _db.Messages.Where(ms => ms.Id == Id).FirstOrDefault();
            var account = await userManager.GetUserAsync(User);
            _db.ChildMessages.Attach(new ChildMessage { Account = account, Content = Content, CommentDate = DateTime.Now, Message = ms });
            _db.SaveChanges();

            return Redirect(ReturnUrl);

        }



    }
}