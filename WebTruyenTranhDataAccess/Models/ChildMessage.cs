using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class ChildMessage
    {
        public long Id { get; set; }

        [MaxLength(1000, ErrorMessage = "Please use up to 1000 characters.")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Content { get; set; }

        public long AccountId { get; set; }
        public Account Account { get; set; }

        public long MessageId { get; set; }
        public Message Message { get; set; }

        public DateTime CommentDate { get; set; }
    }
}