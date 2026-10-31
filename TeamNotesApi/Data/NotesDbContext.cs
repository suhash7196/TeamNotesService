using Microsoft.EntityFrameworkCore;
using TeamNotesApi.Models;

namespace TeamNotesApi.Data
{
    public class NotesDbContext : DbContext
    {
        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure multi-relation behavior to prevent cascade cycle errors
            modelBuilder.Entity<Note>()
                .HasOne(n => n.Team)
                .WithMany()
                .HasForeignKey(n => n.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Note>()
                .HasOne(n => n.CreatorUser)
                .WithMany()
                .HasForeignKey(n => n.CreatorUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}