using FeedbackApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FeedbackApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Upvote> Upvotes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User - Feedback ilişkisi (NoAction)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Feedbacks)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // User - Comment ilişkisi (NoAction)
            modelBuilder.Entity<User>()
                .HasMany(u => u.Comments)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Feedback - Comment ilişkisi (Cascade)
            modelBuilder.Entity<Feedback>()
                .HasMany(f => f.Comments)
                .WithOne(c => c.Feedback)
                .HasForeignKey(c => c.FeedbackId)
                .OnDelete(DeleteBehavior.Cascade);

            // Category - Feedback ilişkisi (NoAction)
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Feedbacks)
                .WithOne(f => f.Category)
                .HasForeignKey(f => f.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }
    }
}