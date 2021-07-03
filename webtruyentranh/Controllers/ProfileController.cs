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
        private ComicContext db;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;

        public ProfileController(UserManager<Account> userManager, SignInManager<Account> signInManager, ComicContext db)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.db = db;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Getme()
        {
            var account = db.Accounts.Include(A=>A.Profile).FirstOrDefault(a => a.UserName == User.Identity.Name);

            
            var novels = db.Novels.Where(n => n.Account.Id == account.Id).ToList();
            ViewBag.profile = account.Profile;
            ViewBag.novels= novels;
            ViewBag.isany = novels.Any();
            ViewBag.accountId = account.Id;
            ViewBag.isMe = true;


            return View("Getprofile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Getprofile(long Id)
        {

            var profile = db.Profiles.Find(Id);
            var account = db.Accounts.FirstOrDefault(a => a.Profile.Id == Id);
            var novels = db.Novels.Where(n => n.Account.Id == account.Id).ToList();
            ViewBag.profile = profile;
            ViewBag.novels = novels;
            ViewBag.isany = novels.Any();
            ViewBag.isMe = false;
            ViewBag.accountId = account.Id;
            return View("Getprofile");
        }

        [HttpGet]
        public IActionResult Message ()

        {
            return PartialView("_Messageparticalview");
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Message(String Content , long Id)
        {
            var SenderAccount = await userManager.GetUserAsync(User);
            var ReciveAccount = userManager.Users.FirstOrDefault(a => a.Id == Id);
            Debug.WriteLine(Content);
            Debug.WriteLine(SenderAccount.UserName);
            Debug.WriteLine(ReciveAccount.UserName);
            db.Messages.Attach(new Message()
            {

                Sender = SenderAccount,
                Receiver = ReciveAccount,
                Content = Content,
                CreateDate=DateTime.Now

            });
            db.SaveChanges();
            return RedirectToAction("Getme","Profile");
        }

        public IActionResult Reply()
        {
            return null;
        }

    }
}