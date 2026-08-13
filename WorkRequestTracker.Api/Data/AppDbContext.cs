using Microsoft.EntityFrameworkCore;
using WorkRequestTracker.Api.Models;

namespace WorkRequestTracker.Api.Data
{
    // This class represents our connection to the database.
    // It's the EF Core equivalent of "here's my table(s)".
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // This tells EF Core: "there is a table called WorkRequests,
        // and it maps to the WorkRequest class."
        public DbSet<WorkRequest> WorkRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Store enums as strings in SQL
            modelBuilder.Entity<WorkRequest>()
                .Property(w => w.Priority)
                .HasConversion<string>();

            modelBuilder.Entity<WorkRequest>()
                .Property(w => w.Status)
                .HasConversion<string>();
        }
    }
}