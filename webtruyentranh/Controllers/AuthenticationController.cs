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

public class AuthenticationController : Controller
    {
    private ComicContext db;
    private readonly UserManager<Account> userManager;
    private readonly SignInManager<Account> signInManager;  
    public AuthenticationController(UserManager<Account>  userManager,SignInManager<Account> signInManager)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;

    }
    [HttpGet]
  
    public IActionResult Index()
    {
        ViewData["Islogin"] = true;
        return View();
    }
        [HttpPost]
        public async Task< IActionResult> Login( Login_viewmodel login){
    
        if (ModelState.IsValid)
            {
            var result = await signInManager.PasswordSignInAsync(userName: login.UserName, password: login.Password, isPersistent:false,false) ;
            if (result.Succeeded)
            {

                return RedirectToAction("index", "Home");
            }
            ModelState.AddModelError("", "invalid login infomation");
            }
        ViewData["Islogin"] = true;
        return View("Index");
            
        }
        [HttpPost]
        public async Task< IActionResult> Register(Register_viewmodel register){

        if (ModelState.IsValid)
            {
            var account = new Account() { UserName=register.R_UserName, Email=register.R_Email,EmailConfirmed=true };
            var result = await userManager.CreateAsync(account, register.R_Password);
          
            if (result.Succeeded)
                {
                signInManager.SignInAsync(account, isPersistent: false);
                return RedirectToAction("index", "Home");
                }
            foreach (var errror in result.Errors)
                {
                ModelState.AddModelError("", errror.Description);
                }

            }
     
        ViewData["Islogin"] = false;
        return View("Index");  
        }

    
    public async Task<IActionResult> Logout(){
       await signInManager.SignOutAsync();
        return RedirectToAction("index", "Home");

    }





    }

