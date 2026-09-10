using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SL.Infrastructures.EntityFramework.Entities;
using SL.Infrastructures.EntityFramework.Entities.SysEntities;
using SL.Infrastructures.EntityFramework.SeedData;

namespace SL.Infrastructures.EntityFramework
{
    public class SnapLifeContext : IdentityDbContext<AspNetUsers, AspNetRoles, string>
    {
        public SnapLifeContext(DbContextOptions<SnapLifeContext> options) : base(options)
        {
        }

        // System & Media
        public virtual DbSet<SysFile> SysFiles { get; set; } = null!;
        public virtual DbSet<SysActivityLog> SysActivityLogs { get; set; } = null!;
        public virtual DbSet<SysConfiguration> SysConfigurations { get; set; } = null!;

        // Auth & Devices
        public virtual DbSet<UserRefreshToken> UserRefreshTokens { get; set; } = null!;
        public virtual DbSet<UserDevice> UserDevices { get; set; } = null!;

        // Social Core
        public virtual DbSet<Post> Posts { get; set; } = null!;
        public virtual DbSet<PostMedia> PostMedias { get; set; } = null!;
        public virtual DbSet<Story> Stories { get; set; } = null!;
        public virtual DbSet<StoryView> StoryViews { get; set; } = null!;
        public virtual DbSet<Hashtag> Hashtags { get; set; } = null!;
        public virtual DbSet<PostHashtag> PostHashtags { get; set; } = null!;
        public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; } = null!;

        // Interactions
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Reaction> Reactions { get; set; } = null!;
        public virtual DbSet<UserRelationship> UserRelationships { get; set; } = null!;
        public virtual DbSet<Bookmark> Bookmarks { get; set; } = null!;
        public virtual DbSet<Notification> Notifications { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SysFile>().ToTable("MediaFiles");

            // Composite Key for PostHashtag
            modelBuilder.Entity<PostHashtag>()
                .HasKey(ph => new { ph.PostId, ph.HashtagId });

            modelBuilder.Entity<PostHashtag>()
                .HasOne(ph => ph.Post)
                .WithMany(p => p.PostHashtags)
                .HasForeignKey(ph => ph.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostHashtag>()
                .HasOne(ph => ph.Hashtag)
                .WithMany(h => h.PostHashtags)
                .HasForeignKey(ph => ph.HashtagId)
                .OnDelete(DeleteBehavior.Cascade);

            // Unique index for Hashtag Tag
            modelBuilder.Entity<Hashtag>()
                .HasIndex(h => h.Tag)
                .IsUnique();

            // Unique index for Reaction: 1 user can only have 1 reaction per target (Post/Comment/Story)
            modelBuilder.Entity<Reaction>()
                .HasIndex(r => new { r.UserId, r.TargetType, r.TargetId })
                .IsUnique();

            // Self-referencing relationship for Nested Comments
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Comment to Post
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Comment to User
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bookmark: Restrict on Post/User to avoid multiple cascade paths in SQL Server
            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.User)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bookmark>()
                .HasOne(b => b.Post)
                .WithMany()
                .HasForeignKey(b => b.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // User Relationship: Source and Target users (Restrict both to avoid cascade cycle)
            modelBuilder.Entity<UserRelationship>()
                .HasOne(ur => ur.SourceUser)
                .WithMany()
                .HasForeignKey(ur => ur.SourceUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRelationship>()
                .HasOne(ur => ur.TargetUser)
                .WithMany()
                .HasForeignKey(ur => ur.TargetUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRelationship>()
                .HasIndex(ur => new { ur.SourceUserId, ur.TargetUserId, ur.Type })
                .IsUnique();

            // Notification: Receiver and Actor (Actor Restrict)
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Receiver)
                .WithMany()
                .HasForeignKey(n => n.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Actor)
                .WithMany()
                .HasForeignKey(n => n.ActorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Bookmark unique per user and post
            modelBuilder.Entity<Bookmark>()
                .HasIndex(b => new { b.UserId, b.PostId })
                .IsUnique();

            // Indexes for Feed performance
            modelBuilder.Entity<Post>()
                .HasIndex(p => new { p.UserId, p.CreatedDate });

            modelBuilder.Entity<Post>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Story>()
                .HasIndex(s => new { s.UserId, s.ExpiresAt });

            // StoryView: ViewerUser Restrict to avoid multiple cascade paths
            modelBuilder.Entity<StoryView>()
                .HasOne(sv => sv.ViewerUser)
                .WithMany()
                .HasForeignKey(sv => sv.ViewerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StoryView>()
                .HasOne(sv => sv.Story)
                .WithMany(s => s.StoryViews)
                .HasForeignKey(sv => sv.StoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StoryView>()
                .HasIndex(sv => new { sv.StoryId, sv.ViewerUserId })
                .IsUnique();

            // Reaction to User Restrict
            modelBuilder.Entity<Reaction>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Post to ExpenseCategory
            modelBuilder.Entity<Post>()
                .HasOne(p => p.ExpenseCategory)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.ExpenseCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Seed default expense categories using static seed data class
            modelBuilder.Entity<ExpenseCategory>().HasData(ExpenseCategorySeedData.GetAll());
        }

        public override int SaveChanges()
        {
            SetAuditFields();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetAuditFields()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is AuditEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (AuditEntity)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate ??= DateTime.UtcNow;
                    entity.IsActive ??= true;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdatedDate = DateTime.UtcNow;
                }
            }
        }
    }
}
