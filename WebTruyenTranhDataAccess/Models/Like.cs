using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Like
    {
        public long NovelId { get; set; }
        public Novel Novel { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
    }
}