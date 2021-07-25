using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using webtruyentranh.Models;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;

namespace webtruyentranh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ComicContext _db;

        public HomeController(ILogger<HomeController> logger, ComicContext db)
        {
            _logger = logger;
            this._db = db;
        }

        public IActionResult Index()
        {
            //var novels = _db.Novels.Include(a => a.Likes).Include(b => b.Account).ThenInclude(d => d.Profile).ToList();
            // ViewBag.countCreator = _db.Accounts.Include(b => b.Novels).ToList().Count;
            // ViewBag.countNovels = novels.ToList().Count;
            // ViewBag.Novel1= novels.OrderByDescending(c => c.LikeCount).Skip(0).Take(1).First();
            // ViewBag.Novel2 = novels.OrderByDescending(c => c.LikeCount).Skip(1).Take(1).First();
            // ViewBag.Novel3 = novels.OrderByDescending(c => c.LikeCount).Skip(2).Take(1).First();
            // ViewBag.Novel4 = novels.OrderByDescending(c => c.LikeCount).Skip(3).Take(1).First();
            // ViewBag.Novel5 = novels.OrderByDescending(c => c.LikeCount).Skip(4).Take(1).First();
            // ViewBag.Novel6 = novels.OrderByDescending(c => c.LikeCount).Skip(5).Take(1).First();
            // ViewBag.Novel7 = novels.OrderByDescending(c => c.LikeCount).Skip(6).Take(1).First();
            // ViewBag.Novel8 = novels.OrderByDescending(c => c.LikeCount).Skip(7).Take(1).First();
            // ViewBag.Novel9 = novels.OrderByDescending(c => c.LikeCount).Skip(8).Take(1).First();

            // //ViewBag.Featurednovel1 = novels.OrderBy(n => n.LastestUpdate).Skip(0).Take(1);
            // //ViewBag.Featurednovel2 = novels.OrderBy(n => n.LastestUpdate).Skip(1).Take(1);
            // //ViewBag.Featurednovel3 = novels.OrderBy(n => n.LastestUpdate).Skip(2).Take(1);

            // var novelFeature = novels.OrderByDescending(n => n.LastestUpdate).Skip(0).Take(3).ToList();
            // return View(novelFeature);
            return View("about");
        }

        [Route("About")]
        public IActionResult About()
        {
            return View();
        }

        [Route("Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Creators()
        {
            var listCreator = _db.Profiles.Include(a => a.Account).ThenInclude(b => b.Novels).OrderByDescending(c => c.Account.Novels.Count).Take(8).ToList();
            return View(listCreator);
        }
    }
}