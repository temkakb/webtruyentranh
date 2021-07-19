using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;

using WebTruyenTranhDataAccess.Models;

namespace webtruyentranh.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly ComicContext _context;
        private readonly UserManager<Account> userManager;
        private readonly SignInManager<Account> signInManager;

        public EpisodeController(ComicContext context, UserManager<Account> userManager, SignInManager<Account> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
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
          
                var episode = _context.Episodes.Where(ep => ep.Novel.Slugify.Equals(novelSlugify)).Where(ep => ep.EpisodeNumber == episodenumber).FirstOrDefault();
                if(episode==null)
                return PartialView("~/Views/Shared/_notfound.cshtml");
                ViewBag.epcount = _context.Episodes.Where(ep => ep.Novel.Slugify == novelSlugify).Count();
                var novel = _context.Novels.Include(m => m.Likes).Include(m => m.Genres).FirstOrDefault(p => p.Slugify == novelSlugify);
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



        [HttpPost]
        public JsonResult GetEpisodes(int id)
        {
            var novel = _context.Novels.Include(n => n.Episodes).FirstOrDefault(n => n.Id == id);
            var SerialList = new List<Episode>();
            novel.Episodes.ForEach(e => SerialList.Add(new Episode()
            {
                Id = e.Id,
                EpisodeNumber = e.EpisodeNumber,
                Comments = e.Comments,
                Content = e.Content,
                Notifications = e.Notifications,
                Novel = e.Novel,
                Views = e.Views,
                Title = e.Title
            }));
            return Json(JsonConvert.SerializeObject(SerialList, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                PreserveReferencesHandling = PreserveReferencesHandling.Arrays,
                Formatting = Formatting.Indented
            }));
        }
    }
}