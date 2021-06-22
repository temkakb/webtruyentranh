using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// pending
///
/// </summary>
namespace WebTruyenTranhDataAccess.Models
{
    public class ChildComment
    {
        public long Id { get; set; }

        [MaxLength(1000, ErrorMessage = "Please use up to 1000 characters.")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Content { get; set; }

        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long CommentId { get; set; }
        public Comment Comment { get; set; }

        public DateTime CommentDate { get; set; }
    }
}