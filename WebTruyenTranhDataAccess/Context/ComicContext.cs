using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTruyenTranhDataAccess.Models;

namespace WebTruyenTranhDataAccess.Context
{
    public class ComicContext : DbContext
    {
        public ComicContext(DbContextOptions<ComicContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Entity Subscription
            modelBuilder.Entity<Subscription>().HasKey(sc => sc.Id);

            modelBuilder.Entity<Subscription>()
                .HasOne<Account>(sub => sub.Subscriber)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(sub => sub.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription>()
                .HasOne<Novel>(sub => sub.Novel)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(sub => sub.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            //Entity Like
            modelBuilder.Entity<Like>().HasKey(lk => new { lk.AccountId, lk.NovelId });

            modelBuilder.Entity<Like>()
                .HasOne<Account>(sub => sub.Account)
                .WithMany(s => s.Likes)
                .HasForeignKey(sub => sub.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like>()
                .HasOne<Novel>(sub => sub.Novel)
                .WithMany(s => s.Likes)
                .HasForeignKey(sub => sub.NovelId)
                .OnDelete(DeleteBehavior.Cascade);

            // Entity Comment
            modelBuilder.Entity<Comment>().HasKey(cm => cm.Id);

            modelBuilder.Entity<Comment>()
                .HasOne<Account>(sub => sub.Account)
                .WithMany(s => s.Comments)
                .HasForeignKey(sub => sub.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne<Episode>(sub => sub.Episode)
                .WithMany(s => s.Comments)
                .HasForeignKey(sub => sub.EpisodeId)
                .OnDelete(DeleteBehavior.NoAction);
            //ChildComment
            modelBuilder.Entity<ChildComment>().HasKey(cm => cm.Id);

            modelBuilder.Entity<ChildComment>()
                .HasOne<Account>(sub => sub.Account)
                .WithMany(s => s.ChildComments)
                .HasForeignKey(sub => sub.AccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ChildComment>()
                .HasOne<Comment>(sub => sub.Comment)
                .WithMany(s => s.ChildComments)
                .HasForeignKey(sub => sub.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            //Notification
            modelBuilder.Entity<Notification>().HasKey(lk => lk.Id);

            modelBuilder.Entity<Notification>()
                .HasOne<Account>(sub => sub.Account)
                .WithMany(s => s.Notifications)
                .HasForeignKey(sub => sub.TargetAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne<Episode>(sub => sub.Episode)
                .WithMany(s => s.Notifications)
                .HasForeignKey(sub => sub.NewEpisodeId)
                .OnDelete(DeleteBehavior.Cascade);

            //Message
            modelBuilder.Entity<Message>().HasKey(lk => lk.Id);

            modelBuilder.Entity<Message>()
                .HasOne<Account>(sub => sub.Receiver)
                .WithMany(s => s.MessagesReceived)
                .HasForeignKey(sub => sub.ReceiverAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Message>()
                .HasOne<Account>(sub => sub.Sender)
                .WithMany(s => s.MessagesSent)
                .HasForeignKey(sub => sub.SenderAccountId)
                .OnDelete(DeleteBehavior.NoAction);

            ///
            modelBuilder.Entity<ChildMessage>().HasKey(lk => lk.Id);

            modelBuilder.Entity<ChildMessage>()
                .HasOne<Account>(sub => sub.Account)
                .WithMany(s => s.ChildMessages)
                .HasForeignKey(sub => sub.AccountId);

            modelBuilder.Entity<ChildMessage>()
                .HasOne<Message>(sub => sub.Message)
                .WithMany(s => s.ChildMessages)
                .HasForeignKey(sub => sub.MessageId);

            //modelBuilder.Entity("WebTruyenTranhDataAccess.Models.Novel", b =>
            //{
            //    b.HasOne("WebTruyenTranhDataAccess.Models.Account", "Account")
            //        .WithMany("Novels")
            //        .HasForeignKey("AccountId");

            //    b.Navigation("Account");
            //});

            //modelBuilder.Entity<Account>()
            //     .HasMany(s => s.SubscribedNovels)
            //     .WithMany(s => s.SubscribedAccount)
            //    .UsingEntity<Subscription>
            //    (sub => sub.HasOne<Novel>().WithMany(),
            //     sub => sub.HasOne<Account>().WithMany())
            //    .Property(sub => sub.ExpirationDate);
        }

        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Novel> Novels { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<ChildComment> ChildComments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChildMessage> ChildMessages { get; set; }
        public DbSet<Episode> Episodes { get; set; }
    }
}