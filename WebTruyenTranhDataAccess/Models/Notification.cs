using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Notification
    {
        public long Id { get; set; }

        public long TargetAccountId { get; set; }
        public Account Account { get; set; }

        public long NewEpisodeId { get; set; }
        public Episode Episode { get; set; }

        public DateTime CreateDate { get; set; }
        public bool IsRead { get; set; }

        [MaxLength(100, ErrorMessage = "Please use up to 100 characters.")]
        [Column(TypeName = "nvarchar(100)")]
        public string Content { get; set; }
    }
}