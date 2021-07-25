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
using Microsoft.AspNetCore.Identity;
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

        
        [Route("novel/{novelId}/episode/Create")]
        [HttpGet]
        [Authorize]
        public IActionResult Create(long novelId)
        {
            var model = new Episode();
            model.NovelId = novelId;
            return View(model);
        }

        [Route("novel/{novelId}/episode/Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
    
        public IActionResult Create(Episode model)
        {
            
            if (ModelState.IsValid)
            { 
                var ep = new Episode();
                var lastEpisode = _context.Episodes.Where(m => m.NovelId == model.NovelId).OrderBy(p => p.EpisodeNumber).LastOrDefault();
                ep.Id = model.Id;
                ep.NovelId = model.NovelId;
                if(lastEpisode == null)
                {
                    ep.EpisodeNumber = 1;
                }
                else
                {
                    ep.EpisodeNumber = lastEpisode.EpisodeNumber + 1;
                }
                
                ep.Title= model.Title;
                ep.Content = model.Content;
                ep.Views = model.Views;

                try
                {
                    _context.Episodes.Add(ep);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("save_error", "Save Error" + ex.Message);
                    return View(model);
                }
                return RedirectToAction("Sumary", "Dashboard");
            }
            return View(model);
        }


        [HttpGet]
        [Authorize]

        public IActionResult Update(int id)
        {
            var ep = _context.Episodes.FirstOrDefault(m => m.Id == id);
            if(ep == null)
            {
                return RedirectToAction("Sumary", "Dashboard");
            }
            var edit = new Episode();
            edit.Id = ep.Id;
            edit.Title = ep.Title;
            edit.Content = ep.Content;
            return View(edit);
        }

        [HttpPost]
       
        public async Task<IActionResult> UpdateAsync(Episode model)
        {

            if(ModelState.IsValid)
            {
                var account = await userManager.GetUserAsync(User);
                var ep = _context.Episodes.Include(n => n.Novel.Account).FirstOrDefault(m => m.Id == model.Id && m.Novel.Account == account);

                if(ep != null)
                {
                    //ep.Id = model.Id;
                    ep.Title = model.Title;
                    ep.Content = model.Content;

                    // dua episode vao db 
                    _context.Episodes.Attach(ep);
                    _context.Entry(ep).State = EntityState.Modified;

                    _context.SaveChanges();
                }
                return RedirectToAction("Sumary", "Dashboard");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var ep = _context.Episodes.FirstOrDefault(m => m.Id == id);
            if (ep == null)
            {
                return Json(new { success = false, msg = "Cannot Delete!" });
            }
            _context.Episodes.Remove(ep);
            _context.SaveChanges();
            return Json(new { success = true, msg = "" });
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