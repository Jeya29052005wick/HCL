using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        
        }

        public DbSet<Movie> Movies => Set<Movie>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(entity =>
                {
                    // Table configuration
                    entity.ToTable("Movies", table =>
                    {
                        table.HasCheckConstraint(
                            "CK_Movies_DurationMinutes",
                            "[DurationMinutes] > 0"
                        );
                    });

                    // Primary Key
                    entity.HasKey(m => m.MovieId);

                    // MovieId configuration
                    entity.Property(m => m.MovieId)
                          .UseIdentityColumn();

                    // Title configuration
                    entity.Property(m => m.Title)
                          .IsRequired()
                          .HasMaxLength(150);

                    // Description configuration
                    entity.Property(m => m.Description)
                          .HasMaxLength(500);

                    // Duration configuration
                    entity.Property(m => m.DurationMinutes)
                          .IsRequired();

                    // ReleaseDate configuration
                    entity.Property(m => m.ReleaseDate)
                          .HasColumnType("date")
                          .IsRequired();

                    // Language configuration
                    entity.Property(m => m.Language)
                          .IsRequired()
                          .HasMaxLength(50);

                    // Genre configuration
                    entity.Property(m => m.Genre)
                          .IsRequired()
                          .HasMaxLength(50);
                });
        }
    }
}