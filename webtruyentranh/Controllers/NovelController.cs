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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using webtruyentranh.Viewmodels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using webtruyentranh.Utility;

namespace webtruyentranh.Controllers
{


    public class NovelController : Controller
    {
        private readonly ComicContext _db;
        private readonly UserManager<Account> userManager;
        public NovelController(ComicContext _db, UserManager<Account> userManager)
        {
            this._db = _db;
            this.userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Getnovels()
        {
            var genre = _db.Genres.ToList();

            return View(genre);
        }
        [HttpGet]

        public JsonResult RequestItems(long ge, String q, int pagination, string search)
        {
            int staticnum = pagination * 5;
            int count;
            IEnumerable<Novel> novels;
            if (ge == 0)
            {
                novels = _db.Novels.Include(n => n.Genres).ToList(); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                count = novels.Count();
                novels = novels.Skip(staticnum - 5).Take(staticnum);
            }
            else
            {
                if (q.Equals("POPULAR"))
                {
                    novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList().OrderByDescending(n => n.LikeCount); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                    count = novels.Count();
                    novels = novels.Skip(staticnum - 5).Take(staticnum);
                }
                if (q.Equals("FRESH"))
                {
                    novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList().OrderBy(n => n.LastestUpdate); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                    count = novels.Count();
                    novels = novels.Skip(staticnum - 5).Take(staticnum);
                }
                novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)).ToList(); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                count = novels.Count();
                novels = novels.Skip(staticnum - 5).Take(staticnum);
            }
            var Featurednovel = _db.Novels.Include(n => n.Genres).ToList().OrderBy(n => n.LastestUpdate).Skip(0).Take(2).ToList();
            //ViewBag.pagination = pagination+1;
            return Json(JsonConvert.SerializeObject(new { Novels = novels, countresult = count, Featurednovel = Featurednovel }, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                Formatting = Formatting.Indented
            }));
        }
        [Authorize]
        public async Task<IActionResult> DoSubscription(long Id)
        {
            try
            {
                var account = await userManager.GetUserAsync(User);
                var any = _db.Subscriptions.Where(s => s.AccountId == account.Id).Where(s => s.NovelId == Id).SingleOrDefault();
                if (any == null)
                {
                    _db.Subscriptions.Add(new Subscription
                    {
                        AccountId = account.Id,
                        NovelId = Id,
                        ExpirationDate = DateTime.Now
                    });
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok();
        }
        public async Task<IActionResult> UnSubscription(long Id)
        {
            try
            {
                var account = await userManager.GetUserAsync(User);
                var any = _db.Subscriptions.Where(s => s.AccountId == account.Id).Where(s => s.NovelId == Id).SingleOrDefault();
                if (any != null)
                {
                    _db.Remove(any);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok();
        }
        public async Task<IActionResult> Dolike(long Id)
        {
            try
            {
                var account = await userManager.GetUserAsync(User);
                var any = _db.Likes.Where(l => l.AccountId == account.Id).Where(l => l.NovelId == Id).SingleOrDefault();
                if (any == null)
                {
                    _db.Likes.Add(new Like
                    {
                        AccountId = account.Id,
                        NovelId = Id

                    });
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok();
        }
        public async Task<IActionResult> UnLike(long Id)
        {
            try
            {
                var account = await userManager.GetUserAsync(User);
                var any = _db.Likes.Where(l => l.AccountId == account.Id).Where(l => l.NovelId == Id).SingleOrDefault();
                if (any != null)
                {
                    _db.Remove(any);
                    _db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }
            return Ok();

        }
        
        [HttpGet]
        public IActionResult CreateNovels()
        {
            var item = _db.Genres.Select(x => new SelectListItem()
            {
                Text = x.GenreName,
                Value = x.Id.ToString(),
            }).ToList();
            var gn = new UploadImage_Viewmodel()
            {
                genres = item
            };
                 return View(gn);

        }
        
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> CreateNovelsAsync(UploadImage_Viewmodel createnovel)
        {
            if (ModelState.IsValid)

            {
                var account = await userManager.GetUserAsync(User);
                var nv = new Novel();
                //nv.Id = createnovel.Id;
                nv.Title = createnovel.Title;
                nv.Description = createnovel.Description;
                nv.LikeCount = 0;
                nv.LastestUpdate = DateTime.Now;
                nv.Account = account;
                nv.Thumbnail = await Cloudinary_Utility.uploadavartar(createnovel.Thumbnail);
                nv.BookCover = await Cloudinary_Utility.uploadavartar(createnovel.Bookcover);
                nv.Banner = await Cloudinary_Utility.uploadavartar(createnovel.Banner);

                try
                {
                    _db.Attach(account);
                    _db.Novels.Add(nv);
                    _db.SaveChanges();
            }

                catch (Exception ex)
            {
                ModelState.AddModelError("save_error", "Save Error" + ex.Message);
                return View(createnovel);
            }
            return RedirectToAction("Sumary", "Dashboard");
            }
            return View(createnovel);
        }

    }


}

        


 