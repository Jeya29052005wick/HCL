using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<Booking> BookingQuery()
        {
            return _context.Bookings
                .Include(b => b.Show)
                .Include(b => b.BookingSeats)
                    .ThenInclude(bs => bs.Seat);
        }

        public async Task<List<Booking>> GetAllAsync(
            int? showId,
            string? status)
        {
            var query = BookingQuery();

            if (showId.HasValue)
            {
                query = query.Where(b => b.ShowId == showId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(b => b.Status == status);
            }

            return await query
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task<Booking?> GetByIdAsync(int bookingId)
        {
            return await BookingQuery()
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);
        }

        public async Task<List<Booking>> GetByShowIdAsync(int showId)
        {
            return await BookingQuery()
                .Where(b => b.ShowId == showId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();
        }

        public async Task AddWithSeatsAsync(
            Booking booking,
            List<int> seatIds)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Bookings.AddAsync(booking);

                // Save first to generate BookingId.
                await _context.SaveChangesAsync();

                // Add the selected seats to the booking.
                await _context.BookingSeats.AddRangeAsync(
                    booking.BookingSeats);

                // Mark the selected seats as booked for this show.
                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Booked";
                }

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task CancelAsync(Booking booking)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                var seatIds = booking.BookingSeats
                    .Select(bs => bs.SeatId)
                    .ToList();

                var showSeats = await _context.ShowSeats
                    .Where(ss =>
                        ss.ShowId == booking.ShowId &&
                        seatIds.Contains(ss.SeatId))
                    .ToListAsync();

                foreach (var showSeat in showSeats)
                {
                    showSeat.Status = "Available";
                }

                booking.Status = "Cancelled";

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}