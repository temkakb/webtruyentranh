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
namespace webtruyentranh.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly ILogger<HomeController> _logger;
        public ComicContext _db;
        
        public HomeController(ILogger<HomeController> logger,ComicContext db)
        {
            _logger = logger;
             this._db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
       
        public IActionResult Creator()
        {
            var listCreator = _db.Accounts.Include(a => a.Profile).Include(b=>b.Novels).OrderByDescending(c=>c.Novels.Count).Take(8).ToList();
            return View(listCreator);
        }
       
    }
}
