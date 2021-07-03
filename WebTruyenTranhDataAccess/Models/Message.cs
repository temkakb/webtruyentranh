using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Message
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(1000, ErrorMessage = "Please use up to 1000 characters.")]
        [Column(TypeName = "nvarchar(1000)")]
        public string Content { get; set; }

        public long SenderAccountId { get; set; }
        public Account Sender { get; set; }

        public long ReceiverAccountId { get; set; }
        public Account Receiver { get; set; }

        public DateTime CreateDate { get; set; }

        public List<ChildMessage> ChildMessages { get; set; }
    }
}