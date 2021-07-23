using Microsoft.AspNetCore.Authorization;
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

        /**-----------------------------------------------------comment-------------------------------------------------------------**/
        public JsonResult GetComments (long Id,int pagination)
        {
            var staticnum = pagination * 5;
            var listcmt = _context.Comments.Include(c => c.ChildComments).ThenInclude(child => child.Account.Profile).Include(c => c.Account.Profile).Where(c => c.EpisodeId == Id)
                .OrderByDescending(d => d.CommentDate).Skip(staticnum-5).Take(5).ToList();
            return Json(JsonConvert.SerializeObject(new { listcmt = listcmt }, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Postcomment (long Id,string comment,string ReturnUrl)
        {
            try
            {
                var account = await userManager.GetUserAsync(User);    
                _context.Comments.Attach(new Comment()
                {
                    Account=account,
                    CommentDate=DateTime.Now,
                    Content=comment,
                    EpisodeId=Id
                   
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Sorry, can't add comment");
            }
            return Redirect(ReturnUrl);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyComment(long Id, String Content, String ReturnUrl) //post reply and return

        { //get form reply for the mss
           
            Comment cmt = _context.Comments.Where(cmt => cmt.Id == Id).FirstOrDefault();
            if (cmt == null)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");

            }
            var account = await userManager.GetUserAsync(User);
            _context.ChildComments.Attach(new ChildComment { Account = account, Content = Content, CommentDate = DateTime.Now, Comment = cmt });
            _context.SaveChanges();
            return Redirect(ReturnUrl);
        }


    }
}