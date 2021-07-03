using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace webtruyentranh.Views.Shared.Components.Sendmessage
{
    public class Firstloadmessage : ViewComponent
    {
        private ComicContext db;
        public Firstloadmessage(ComicContext db)
        {
            this.db = db;

        }
        public IViewComponentResult Invoke (long Id)
        {
            Debug.WriteLine(Id);
            var listms = db.Messages.Include(m => m.ChildMessages).ThenInclude(child=>child.Account.Profile).Include(m => m.Sender.Profile).Where(m => m.ReceiverAccountId == Id)
                .OrderByDescending(d=>d.CreateDate).Skip(0).Take(10);
            
            return View("_messageparticalview.cshtml", listms);
        }
    }
}
