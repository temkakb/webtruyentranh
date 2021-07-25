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
using Microsoft.AspNetCore.Identity;
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
                novels = _db.Novels.Include(n => n.Genres).Include(n=>n.Likes); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                count = novels.Count();            
            }
            else
            {
                novels = _db.Novels.Include(n => n.Genres).Where(n => n.Genres.Any(g => g.Id == ge)); if (search != null) { novels = novels.Where(n => n.Title.Contains(search) || n.Genres.Any(g => g.GenreName.Contains(search))); }
                count = novels.Count();            
            }
            if (q.Equals("POPULAR"))
            {               
                novels = novels.OrderByDescending(n => n.LikeCount);
            }
            if (q.Equals("FRESH"))
            {
                novels = novels.OrderBy(n => n.LastestUpdate);
            }
            novels = novels.Skip(staticnum - 5).Take(5).ToList();  
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
        [Authorize]
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
        
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> CreateNovelsAsync(UploadImage_Viewmodel createnovel)
        {
            if (ModelState.IsValid)

            {

                var account = await userManager.GetUserAsync(User);
                var nv = new Novel();
                nv.Title = createnovel.Title;
                nv.Description = createnovel.Description;
                nv.LikeCount = 0;
                nv.LastestUpdate = DateTime.Now;
                nv.Account = account;
                Task task1 = new Task(async()=> { nv.Thumbnail = await Cloudinary_Utility.uploadavartar(createnovel.Thumbnail); });
                Task task2 = new Task(async () => { nv.BookCover = await Cloudinary_Utility.uploadavartar(createnovel.Bookcover); });
                Task task3 = new Task(async () => { nv.Banner = await Cloudinary_Utility.uploadavartar(createnovel.Banner); });
               
                task1.Start();
                task2.Start();
                task3.Start();
                task1.Wait();
                task2.Wait();
               
                nv.Slugify = Slugify.GenerateSlug(nv.Title);
                var genresIds = createnovel.genres.Where(x => x.Selected)
                                                .Select(y => y.Value);

                var genres = _db.Genres.Where(g => genresIds.Contains(g.Id.ToString())).ToList();
                
                try
                {
                    _db.Attach(account);
                    _db.Novels.Add(nv);
                    nv.Genres = (List<Genre>)genres;
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

        [Authorize]
        [HttpGet]

        public async Task<IActionResult> UpdateNovel(long id)
        {
            try
            {
                var novel = _db.Novels.Include(p => p.Account).Include(h=>h.Genres).Where(novel=>novel.Id==id).First();
                //lay tat ca the loai duoi db len
                var item = _db.Genres.Select(x =>new SelectListItem()
                    {
                        Text = x.GenreName,
                        Value = x.Id.ToString(),

                    }
                ).ToList();

                foreach (var genredb in item) //xet tat ca the laoi duoi db
                {
                    foreach (var genre in novel.Genres) //xet tat ca the loai cua novel dc chon
                    {
                        if (genredb.Value.Equals(genre.Id.ToString())) //neu id the loai duoi db=id the loai cua novel thi tick
                        {
                            genredb.Selected = true; //hien thi da chon
                        }
                    }                  
                }
                var updateNovelView = new UploadImage_Viewmodel()
                {
                    Id = novel.Id,
                    Title = novel.Title,
                    Description = novel.Description,
                    genres =item,                    
                };
                ViewBag.recentThumbnail = novel.Thumbnail;
                ViewBag.recentBookcover = novel.BookCover;
                ViewBag.recentBanner = novel.Banner;
                return View(updateNovelView);
            }
            catch (Exception ex)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateNovel(UploadImage_Viewmodel updatenovel,long Id)
        {
           

            try
            {
                var account = await userManager.GetUserAsync(User);
                var novel = _db.Novels.Include(q=>q.Account).Include(u=>u.Genres).SingleOrDefault(p => p.Id == Id && p.Account == account);
                ViewBag.recentThumbnail = novel.Thumbnail;
                ViewBag.recentBookcover = novel.BookCover;
                ViewBag.recentBanner = novel.Banner;
                if (ModelState.IsValid)
                {
                    novel.Title = updatenovel.Title;
                    novel.Description = updatenovel.Description;

                    if (updatenovel.Thumbnail != null)
                    {
                        novel.Thumbnail = await Cloudinary_Utility.uploadavartar(updatenovel.Thumbnail);
                    }
                    else if (updatenovel.Bookcover != null)
                    {
                        novel.BookCover = await Cloudinary_Utility.uploadavartar(updatenovel.Bookcover);
                    }
                    else if (updatenovel.Banner != null)
                    {
                        novel.Banner = await Cloudinary_Utility.uploadavartar(updatenovel.Banner);
                    }
                    novel.Slugify = Slugify.GenerateSlug(novel.Title);

                    var genresIds = updatenovel.genres.Where(x => x.Selected)
                                                      .Select(y => y.Value);
                    _db.Attach(novel);
                    novel.Genres.Clear();
                    foreach (var item in genresIds)
                    {
                        novel.Genres.Add(_db.Genres.Find(long.Parse(item)));

                    }
                    _db.Novels.Update(novel);
                    _db.SaveChanges();
                    
                }
                else
                {
                    return View(updatenovel);
                }
            }
            catch (Exception ex)
            {
                 return PartialView("~/Views/Shared/_notfound.cshtml");
            }
            return RedirectToAction("Sumary", "Dashboard");
        }
        [Authorize]
        [HttpGet]
        
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var account = await userManager.GetUserAsync(User);
            var novel = _db.Novels.Include(k=>k.Account).Include(h=>h.Episodes).FirstOrDefault( m => m.Id == id && m.Account== account);
           
            if (novel == null)
            {
                return PartialView("~/Views/Shared/_notfound.cshtml");
            }
            _db.Entry(novel.Account).State = EntityState.Detached;
            _db.Remove(novel);
            _db.SaveChanges();

            return RedirectToAction("Sumary", "Dashboard");
        }
    }
}

        


 