using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTruyenTranhDataAccess.Models
{
    public class Account : IdentityUser<long>
    {
        public override long Id { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [MaxLength(24, ErrorMessage = "Username is up to 24 charaters")]
        [MinLength(8, ErrorMessage = "Username is as least 8 charaters")]
        public override string UserName { get => base.UserName; set => base.UserName = value; }

        public override string Email { get => base.Email; set => base.Email = value; }
        public override bool EmailConfirmed { get => base.EmailConfirmed; set => base.EmailConfirmed = value; }

        public override string PasswordHash { get => base.PasswordHash; set => base.PasswordHash = value; }

        public override string PhoneNumber { get => base.PhoneNumber; set => base.PhoneNumber = value; }
        public override bool PhoneNumberConfirmed { get => base.PhoneNumberConfirmed; set => base.PhoneNumberConfirmed = value; }

        public Profile Profile { get; set; }
        // public Role Role { get; set; }

        public List<Subscription> Subscriptions { get; set; }

        public List<Novel> Novels { get; set; }

        public List<Like> Likes { get; set; }

        public List<Comment> Comments { get; set; }

        public List<ChildComment> ChildComments { get; set; }

        public List<Notification> Notifications { get; set; }

        public List<Message> MessagesSent { get; set; }

        public List<Message> MessagesReceived { get; set; }

        public List<ChildMessage> ChildMessages { get; set; }
    }
}