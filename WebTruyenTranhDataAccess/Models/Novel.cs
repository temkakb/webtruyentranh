using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Novel
    {    
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter novel title")]
        [MaxLength(100, ErrorMessage = "Title is up to 100 charaters")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Title is up to 1000 charaters")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Description { get; set; }

        public string Thumbnail { get; set; }

        public string BookCover { get; set; }

        public string Banner { get; set; }

        public int LikeCount { get; set; }

        public DateTime LastestUpdate { get; set; }
        public String Slugify { get; set; }

        public Account Account { get; set; }

        public List<Genre> Genres { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public List<Like> Likes { get; set; }
    

        public List<Episode> Episodes { get; set; }

        public Novel(long id, string title, string description, string thumbnail,
                string bookCover, string banner, int likeCount, DateTime lastestUpdate)
        {
            Id = id;
            Title = title;
            Description = description;
            Thumbnail = thumbnail;
            BookCover = bookCover;
            Banner = banner;
            LikeCount = likeCount;
            LastestUpdate = lastestUpdate;
        }
    }
}