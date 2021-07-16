using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;

namespace webtruyentranh.Controllers
{
    public class EpisodeController : Controller
    {
        ComicContext db;
        public EpisodeController(ComicContext db) {
            this.db = db;
        }

        [HttpGet]
        public IActionResult getlistepisode()
        {

            return View();
        }

        [HttpGet]
        [Route("novel/{novelSlugify}/episode/{episodenumber}")]       
        public IActionResult Episode(string novelSlugify, int episodenumber)
        {
            var episode = db.Episodes.FirstOrDefault(p => p.EpisodeNumber == episodenumber);
            var novel = db.Novels.Include(m => m.Likes).Include(m => m.Genres).FirstOrDefault(p => p.Slugify == novelSlugify);
            ViewBag.novel = novel;
            return View(episode);
        }

        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Delete()
        {
            return View();
        }
        public IActionResult Update()
        {
            return View();
        }
    }
}
