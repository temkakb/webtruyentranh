using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Episode
    {
        public long Id { get; set; }
        public int EpisodeNumber { get; set; }

        [Column(TypeName = "nvarchar(Max)")]
        public string Content { get; set; }

        public Novel Novel { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Notification> Notifications { get; set; }

        public Episode(long id, int episodeNumber, string content)
        {
            Id = id;
            EpisodeNumber = episodeNumber;
            Content = content;
        }
    }
}