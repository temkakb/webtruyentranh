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
using System.Security.Claims;

public class AuthenticationController : Controller
{
    private ComicContext db;
    private readonly UserManager<Account> userManager;
    private readonly SignInManager<Account> signInManager;
    private readonly RoleManager<IdentityRole<long>> roleManager;

    public AuthenticationController(UserManager<Account> userManager, SignInManager<Account> signInManager, RoleManager<IdentityRole<long>> roleManager, ComicContext db)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.roleManager = roleManager;
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



    /*-------------------------------------------------------Login area-----------------------------------------------------------*/

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
        return View("Index");


    }

    // google handle login
    [AllowAnonymous]
    [HttpGet]
    public IActionResult ExternalLoginGoogle(string returnUrl)
    {
        var redirectUrl = Url.Action("ExternalLoginCallback", "Authentication", new { returnUrl = returnUrl });
        var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }
    public async Task< IActionResult> ExternalLoginCallback(String returnUrl=null,String remoteError=null)

    {
   
        if (remoteError!= null)
        {
            ModelState.AddModelError("", remoteError);
            ViewData["Islogin"] = true;
            return View("Index");

        }
        // get claim
        var infomation = await signInManager.GetExternalLoginInfoAsync();
        if (infomation==null)
        {
            ModelState.AddModelError("", "Error loading login infomation");
            ViewData["Islogin"] = true;
        }
        // check if has
        // no local account 
        // this method login is not tồn tại

        // not matching with any account by this method login
        var signInresult = await signInManager.ExternalLoginSignInAsync(infomation.LoginProvider, infomation.ProviderKey, false, bypassTwoFactor: true);
        if (signInresult.Succeeded)
        {
            return Redirect(returnUrl);

        }   
        // if not
        else
        {
            // take email from that claim
            var email = infomation.Principal.FindFirstValue(ClaimTypes.Email);
            if (email!=null) // email ton tai
            {
                // find account with that email , if has, create login with the infomation of provider, bla bla and that account
                var account = await userManager.FindByEmailAsync(email);
                if (account==null)
                {
                    account = new Account()
                    {
                        UserName = infomation.Principal.FindFirstValue(ClaimTypes.Email).Split('@')[0],
                        Email = infomation.Principal.FindFirstValue(ClaimTypes.Email),
                       EmailConfirmed=true,
                    };
                    db.Profiles.Add(new Profile()
                    {
                        DateJoined = DateTime.Now,
                        DisplayName = account.UserName,
                        Description = "Tell your story !",
                        Account = account,
                        Avartar = "/images/avartar.jpg"

                    }); ;
                  
                       
                    await userManager.CreateAsync(account);
                    await userManager.AddToRoleAsync(account, "Member");
                    db.SaveChanges();
                }    
                await userManager.AddLoginAsync(account, infomation);
                await signInManager.SignInAsync(account, false);
                return Redirect(returnUrl);

            }
            ViewData["Title"] = "Failed (｡╯︵╰｡)	";
            ViewData["message"] = "Please contact support on khonglogindckememay@hecomic.com";
            return View("Authentication_msg");
        }
        
         


    }


    /*-------------------------------------------------------register area-----------------------------------------------------------*/


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
                ViewData["Title"] = "Error";
                ViewData["message"] = "An error occurred while processing your request. Please try again later.";
                return View("Authentication_msg");
            }
            var result = await userManager.ConfirmEmailAsync(account, token);

        if (result.Succeeded)
        {
            try {
                await userManager.AddToRoleAsync(account, "Member");
                db.Profiles.Add(new Profile
                {

                    //  Account = account,
                    DateJoined = DateTime.Now,
                    DisplayName = account.UserName,
                    Description = "Tell your story !",
                    Account = account,
                    Avartar = "/images/avartar.jpg"

                }); ;

                db.SaveChanges();

                ViewData["Title"] = "Succeeded ٩(◕‿◕｡)۶";
                ViewData["message"] = "email has been verified. Login now!";
                return View("Authentication_msg");

            }
      
            catch (Exception ex)
            {
                ViewData["Title"] = "Error";
                ViewData["message"] = "Account confirmed";
                return View();
            }
        }
        ViewData["Title"] = "Error (ಥ﹏ಥ)";
        ViewData["message"] = "Token expired";
        return View("Authentication_msg");


    }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "Home");
        }
    }
