using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using Microsoft.EntityFrameworkCore;
using WebTruyenTranhDataAccess.Models;
using Microsoft.AspNetCore.Identity;

namespace webtruyentranh.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ComicContext _context;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;

        public DashboardController(ComicContext context, UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }

        public IActionResult Sumary()
        {
            return View();
        }

        public ActionResult getDashBoard()
        {
            var novel = _context.Novels.Include(n => n.Likes)
                                      .Include(n => n.Genres)
                                      .Include(n => n.Subscriptions)
                                      .Include(n => n.Episodes)
                                       .ThenInclude(e => e.Comments)
                                        .ThenInclude(c => c.ChildComments)
                                      .ToList();
            int totalLikes = 0;
            int totalViews = 0;
            int totalComments = 0;
            foreach (var item in novel)
            {
                totalLikes += item.LikeCount;
                totalViews += item.Episodes.Sum(e => e.Views);
                item.Episodes.ForEach(e => totalComments += e.totalComment());
            }
            ViewBag.TotalLikes = totalLikes;
            ViewBag.TotalViews = totalViews;
            ViewBag.TotalTotalComments = totalComments;
            return PartialView("_summary", novel);
        }
    }
}