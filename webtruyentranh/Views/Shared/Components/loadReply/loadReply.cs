using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebTruyenTranhDataAccess.Models;

namespace webtruyentranh.Views.Shared.Components.loadReply
{
    public class loadReply : ViewComponent
    {
        public IViewComponentResult Invoke (Profile profile,long Id)
          {
            return View("_replyformpaticalview",profile);

    }
}
}
