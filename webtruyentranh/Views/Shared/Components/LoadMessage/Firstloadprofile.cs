using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using WebTruyenTranhDataAccess.Models;
using Microsoft.EntityFrameworkCore;
namespace webtruyentranh.Views.Shared.Components.LoadMessage

{
    public class Firstloadprofile : ViewComponent
    {
        private ComicContext db;
        public Firstloadprofile(ComicContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke (long ?Id)
        {
            var listmessage = db.Messages.Include(ms => ms.ChildMessages).Include(ms =>ms.Sender).ThenInclude(ac=>ac.p)
                Where(ms => ms.ReceiverAccountId == Id);
            return View("_Messageparticalview.cshtml",listmessage);
        }
    }
}
