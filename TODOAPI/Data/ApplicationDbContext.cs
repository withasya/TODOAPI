using Microsoft.EntityFrameworkCore;
using TODOAPI.Models;

namespace TODOAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Title ve Status için unique constraint
            modelBuilder.Entity<Todo>()
                .HasIndex(t => new { t.Title, t.Status })
                .IsUnique()
                .HasFilter("\"Status\" != 2"); // Status != Completed

            // Diğer validasyonlar
            modelBuilder.Entity<Todo>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<Todo>()
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1000);

            modelBuilder.Entity<Todo>()
                .Property(t => t.Category)
                .HasMaxLength(100);
        }
    }
}