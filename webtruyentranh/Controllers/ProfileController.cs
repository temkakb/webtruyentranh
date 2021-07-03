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

            var profile = _db.Profiles.Include(p => p.Account).FirstOrDefault(p => p.Id == Id);
            var novels = _db.Novels.Where(n => n.Account.Id == profile.Account.Id).ToList();
            ViewBag.profile = profile;
            ViewBag.novels = novels;
            ViewBag.isany = novels.Any();
            ViewBag.isMe = false;

            return View("Getprofile");
        }

        [HttpGet]
        public IActionResult Message()

        {
            return PartialView("_Messageparticalview");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Message(String Content, long Id)
        {
            var SenderAccount = await userManager.GetUserAsync(User);
            var ReciveAccount = userManager.Users.FirstOrDefault(a => a.Id == Id);
            Debug.WriteLine(Content);
            Debug.WriteLine(SenderAccount.UserName);
            Debug.WriteLine(ReciveAccount.UserName);
            _db.Messages.Attach(new Message()
            {

                Sender = SenderAccount,
                Receiver = ReciveAccount,
                Content = Content,
                CreateDate = DateTime.Now

            });
            _db.SaveChanges();
            return RedirectToAction("Getme", "Profile");
        }

        public IActionResult Reply()
        {
            return null;
        }

    }
}