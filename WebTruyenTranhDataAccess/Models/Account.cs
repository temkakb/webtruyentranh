using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Account
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [MaxLength(24, ErrorMessage = "Username is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Username is as least 8 charaters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [MaxLength(24, ErrorMessage = "Password is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Password is as least 8 charaters")]
        public string Password { get; set; }

        public int AccessFailedCount { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public Role Role { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public List<Novel> Novels { get; set; }

        public List<Like> Likes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ChildComment> ChildComments { get; set; }

        public List<Notification> Notifications { get; set; }

        public List<Message> MessagesSent { get; set; }

        public List<Message> MessagesReceived { get; set; }

        public List<ChildMessage> ChildMessages { get; set; }

        public Account(long id, string userName, string password,
                        int accessFailedCount = 0, bool twoFactorEnabled = false)
        {
            Id = id;
            UserName = userName;
            Password = password;
            AccessFailedCount = accessFailedCount;
            TwoFactorEnabled = twoFactorEnabled;
        }
    }
}