using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Genre
    {
        public long Id { get; set; }
        public string GenreName { get; set; }

        public List<Novel> Novels { get; set; }

        public Genre(long id, string genreName)
        {
            Id = id;
            GenreName = genreName;
        }
    }
}