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