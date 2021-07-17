using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebTruyenTranhDataAccess.Context;
using System.Diagnostics;
using WebTruyenTranhDataAccess.Models;
using Newtonsoft.Json;

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
            var genre = _db.Genres.ToList();

            return View(genre);
        }
        [HttpGet]
  
        public JsonResult RequestItems(long ge, String q,int pagination)
        {

            int staticnum = pagination * 5;
            int count;
            Debug.WriteLine(staticnum);
            Debug.WriteLine(ge);
            IEnumerable<Novel> novels;
            if (ge == 0)
            {
                novels = _db.Novels.Include(n => n.Genres).ToList();
                count = novels.Count();
                novels = novels.Skip(staticnum - 5).Take(staticnum);

            }
            else
            {
                if (q.Equals("POPULAR"))
                {
                    novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList().OrderByDescending(n => n.LikeCount);
                     count = novels.Count();
                    novels = novels.Skip(staticnum - 5).Take(staticnum);

                }
                if (q.Equals("FRESH"))
                {
                    novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList().OrderBy(n => n.LastestUpdate);
                    count = novels.Count();
                    novels = novels.Skip(staticnum - 5).Take(staticnum);
                }
                novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList() ;
              count = novels.Count();
                novels = novels.Skip(staticnum - 5).Take(staticnum); 

            }
            var Featurednovel = _db.Novels.Include(n => n.Genres).ToList().OrderBy(n => n.LastestUpdate).Skip(0).Take(2).ToList();
            //ViewBag.pagination = pagination+1;
            return Json(JsonConvert.SerializeObject(new { Novels = novels, countresult=count, Featurednovel= Featurednovel }, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented

            }));

        }
    }

}

