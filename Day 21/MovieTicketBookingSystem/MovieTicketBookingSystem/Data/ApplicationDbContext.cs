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
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingSeat> BookingSeats => Set<BookingSeat>();

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

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("Bookings", table =>
                {
                    table.HasCheckConstraint(
                        "CK_Bookings_TotalAmount",
                        "[TotalAmount] >= 0");
                });

                entity.HasKey(b => b.BookingId);

                entity.Property(b => b.BookingId)
                    .UseIdentityColumn();

                entity.Property(b => b.BookingReference)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasIndex(b => b.BookingReference)
                    .IsUnique();

                entity.Property(b => b.CustomerName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(b => b.CustomerEmail)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(b => b.CustomerPhone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(b => b.BookingDate)
                    .HasColumnType("datetime2")
                    .IsRequired();

                entity.Property(b => b.TotalAmount)
                    .HasPrecision(10, 2)
                    .IsRequired();

                entity.Property(b => b.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasOne(b => b.Show)
                    .WithMany()
                    .HasForeignKey(b => b.ShowId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BookingSeat>(entity =>
            {
                entity.ToTable("BookingSeats", table =>
                {
                    table.HasCheckConstraint(
                        "CK_BookingSeats_Price",
                        "[Price] >= 0");
                });

                entity.HasKey(bs => bs.BookingSeatId);

                entity.Property(bs => bs.BookingSeatId)
                    .UseIdentityColumn();

                entity.Property(bs => bs.Price)
                    .HasPrecision(10, 2)
                    .IsRequired();

                entity.HasOne(bs => bs.Booking)
                    .WithMany(b => b.BookingSeats)
                    .HasForeignKey(bs => bs.BookingId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(bs => bs.Seat)
                    .WithMany()
                    .HasForeignKey(bs => bs.SeatId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}