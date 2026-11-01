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
        public DbSet<Theatre> Theatres => Set<Theatre>();
        public DbSet<Screen> Screens => Set<Screen>();
        public DbSet<Seat> Seats => Set<Seat>();
        public DbSet<Show> Shows => Set<Show>();
        public DbSet<ShowSeat> ShowSeats => Set<ShowSeat>();

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

            modelBuilder.Entity<Theatre>(entity =>
            {
                entity.ToTable("Theatres");

                entity.HasKey(t => t.TheatreId);

                entity.Property(t => t.TheatreId)
                      .UseIdentityColumn();

                entity.Property(t => t.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(t => t.Location)
                      .IsRequired()
                      .HasMaxLength(200);
            });

            modelBuilder.Entity<Screen>(entity =>
            {
                entity.ToTable("Screens");

                entity.HasKey(s => s.ScreenId);

                entity.Property(s => s.ScreenId)
                      .UseIdentityColumn();

                entity.Property(s => s.ScreenName)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.HasOne(s => s.Theatre)
                      .WithMany()
                      .HasForeignKey(s => s.TheatreId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new { s.TheatreId, s.ScreenName })
                      .IsUnique();
            });

            modelBuilder.Entity<Seat>(entity =>
            {
                entity.ToTable("Seats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Seats_SeatNumber",
                        "[SeatNumber] > 0"
                    );
                });

                entity.HasKey(s => s.SeatId);

                entity.Property(s => s.SeatId)
                      .UseIdentityColumn();

                entity.Property(s => s.RowName)
                      .IsRequired()
                      .HasMaxLength(5);

                entity.Property(s => s.SeatNumber)
                      .IsRequired();

                entity.Property(s => s.SeatType)
                      .IsRequired()
                      .HasMaxLength(30);

                entity.HasOne(s => s.Screen)
                      .WithMany()
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(s => new
                {
                    s.ScreenId,
                    s.RowName,
                    s.SeatNumber
                }).IsUnique();
            });

            modelBuilder.Entity<Show>(entity =>
            {
                entity.ToTable("Shows", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Shows_EndTime",
                        "[EndTime] > [StartTime]"
                    );

                    table.HasCheckConstraint(
                        "CK_Shows_TicketPrice",
                        "[TicketPrice] >= 0"
                    );
                });

                entity.HasKey(s => s.ShowId);

                entity.Property(s => s.ShowId)
                      .UseIdentityColumn();

                entity.Property(s => s.StartTime)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(s => s.EndTime)
                      .HasColumnType("datetime2")
                      .IsRequired();

                entity.Property(s => s.TicketPrice)
                      .HasPrecision(10, 2)
                      .IsRequired();

                entity.HasOne(s => s.Movie)
                      .WithMany()
                      .HasForeignKey(s => s.MovieId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(s => s.Screen)
                      .WithMany()
                      .HasForeignKey(s => s.ScreenId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ShowSeat>(entity =>
            {
                entity.ToTable("ShowSeats");

                entity.HasKey(ss => ss.ShowSeatId);

                entity.Property(ss => ss.ShowSeatId)
                      .UseIdentityColumn();

                entity.Property(ss => ss.Status)
                      .IsRequired()
                      .HasMaxLength(20);

                entity.HasOne(ss => ss.Show)
                      .WithMany()
                      .HasForeignKey(ss => ss.ShowId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ss => ss.Seat)
                      .WithMany()
                      .HasForeignKey(ss => ss.SeatId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(ss => new
                {
                    ss.ShowId,
                    ss.SeatId
                }).IsUnique();
            });
        }
    }
}