using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Subscription
    {
        public long Id { get; set; }

        public long AccountId { get; set; }
        public Account Subscriber { get; set; }

        public long NovelId { get; set; }
        public Novel Novel { get; set; }

        public DateTime ExpirationDate { get; set; }
    }
}