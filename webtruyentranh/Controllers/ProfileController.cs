﻿using Microsoft.AspNetCore.Authorization;
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
            Account account = await userManager.GetUserAsync(User);
            Debug.WriteLine(account.Email);
            //  var profile = db.Profiles.Where(p => p.Account.Id == account.Id).SingleOrDefault();

            var novels = db.Novels.Where(n => n.Account.Id == account.Id).ToList();

            ///  ViewBag.profile = profile;
            ViewBag.novels = novels;
            ViewBag.isany = novels.Any();

            return View("Getprofile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Getprofile(long Id)
        {
            return null;
        }

        public IActionResult Sendmessage()
        {
            return null;
        }

        public IActionResult Reply()
        {
            return null;
        }
    }
}