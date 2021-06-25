using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Comment
    {
        public long Id { get; set; }

        [MaxLength(1000, ErrorMessage = "Please use up to 1000 characters.")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Content { get; set; }

        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long EpisodeId { get; set; }
        public Episode Episode { get; set; }

        public DateTime CommentDate { get; set; }

        public List<ChildComment> ChildComments { get; set; }
    }
}