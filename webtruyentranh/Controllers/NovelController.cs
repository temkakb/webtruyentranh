using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTruyenTranhDataAccess.Context;
using System.Diagnostics;

namespace webtruyentranh.Controllers
{
    public class NovelController : Controller
    {
        private readonly ComicContext _db;
        public NovelController(ComicContext _db)
        {
            this._db = _db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Getnovels ()
        {

            return View();
        }
        public IActionResult RequestItems(String ge, int q)
        {
            
            return PartialView("_RequestContainer");

        }
    }
}

