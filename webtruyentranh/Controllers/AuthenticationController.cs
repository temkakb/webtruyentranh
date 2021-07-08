using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using webtruyentranh.Viewmodels;
using Microsoft.AspNetCore.Identity;
using WebTruyenTranhDataAccess.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using webtruyentranh.Utility;

public class AuthenticationController : Controller
{
    private ComicContext db;
    private readonly UserManager<Account> userManager;
    private readonly SignInManager<Account> signInManager;

    public AuthenticationController(UserManager<Account> userManager, SignInManager<Account> signInManager, ComicContext db)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (signInManager.IsSignedIn(User))
        {
            return RedirectToAction("index", "Home");
        }
        ViewData["Islogin"] = true;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(Login_viewmodel login)
    {
        if (ModelState.IsValid)
        {
            var result = await signInManager.PasswordSignInAsync(userName: login.UserName, password: login.Password, isPersistent: false, false);
            if (result.Succeeded)
            {
                return RedirectToAction("index", "Home");
            }
            ModelState.AddModelError("", "invalid login infomation");
        }
        ViewData["Islogin"] = true;
        return RedirectToAction("Getme", "Profile");
    }

    [HttpPost]
    public async Task<IActionResult> Register(Register_viewmodel register)
    {
        var user = await userManager.FindByEmailAsync(register.R_Email);
        if (user != null)
        {
            ModelState.AddModelError("", $"Email {register.R_Email} is already in use");
            ViewData["Islogin"] = false;
            return View("Index");
        }

        if (ModelState.IsValid)
        {
            var account = new Account() { UserName = register.R_UserName, Email = register.R_Email };
            var result = await userManager.CreateAsync(account, register.R_Password);

            if (result.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(account);
                var confirmlink = Url.Action("Confirm_email", "authentication", new { token, email = register.R_Email }, Request.Scheme);
                bool Issendedemail = Email_Utility.send_emailconfirm(register.R_Email, confirmlink);
                if (Issendedemail)
                    return View("email_sent");
                else
                    ModelState.AddModelError("", "Cannot send email, pls register again");
                await userManager.DeleteAsync(account);
            }

            foreach (var errror in result.Errors)
            {
                ModelState.AddModelError("", errror.Description);
            }
        }

        ViewData["Islogin"] = false;
        return View("Index");
    }

    public async Task<IActionResult> Confirm_email(String token, String email)
    {
        var account = await userManager.FindByEmailAsync(email);

        if (account == null)
        {
            ViewData["tittle"] = "Error";
            ViewData["message"] = "An error occurred while processing your request. Please try again later.";
            return View();
        }
        var result = await userManager.ConfirmEmailAsync(account, token);
        if (result.Succeeded)
        {
            db.Profiles.Add(new Profile
            {
                //  Account = account,
                DateJoined = DateTime.Now,
                DisplayName = account.UserName,
                Description = "Tell your story !",
                Avartar = "images/avartar.jpg"
            });
            db.SaveChanges();

            ViewData["Title"] = "Succeeded (￣ω￣)";
            ViewData["message"] = "email has been verified. Login now!";
        }
        return View();
    }

    public String html_email()
    {
        return PartialView("_htmlmailparticalview").ToString();
    }

    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("index", "Home");
    }
}