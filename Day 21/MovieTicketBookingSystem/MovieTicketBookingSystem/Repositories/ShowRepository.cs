using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ShowRepository : IShowRepository
    {
        private readonly ApplicationDbContext _context;

        public ShowRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private IQueryable<Show> ShowQuery()
        {
            return _context.Shows
                .Include(s => s.Movie)
                .Include(s => s.Screen)
                    .ThenInclude(sc => sc.Theatre);
        }

        public async Task<List<Show>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var query = ShowQuery();

            if (date.HasValue)
            {
                var start = date.Value.Date;
                var end = start.AddDays(1);

                query = query.Where(s =>
                    s.StartTime >= start &&
                    s.StartTime < end);
            }

            if (movieId.HasValue)
            {
                query = query.Where(s => s.MovieId == movieId.Value);
            }

            if (theatreId.HasValue)
            {
                query = query.Where(s =>
                    s.Screen.TheatreId == theatreId.Value);
            }

            return await query
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<Show?> GetByIdAsync(int showId)
        {
            return await ShowQuery()
                .FirstOrDefaultAsync(s => s.ShowId == showId);
        }

        public async Task<List<Show>> GetByMovieIdAsync(int movieId)
        {
            return await ShowQuery()
                .Where(s => s.MovieId == movieId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Show>> GetByTheatreIdAsync(int theatreId)
        {
            return await ShowQuery()
                .Where(s => s.Screen.TheatreId == theatreId)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<bool> HasOverlapAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null)
        {
            var query = _context.Shows.Where(s =>
                s.ScreenId == screenId &&
                s.StartTime < endTime &&
                s.EndTime > startTime);

            if (excludeShowId.HasValue)
            {
                query = query.Where(s =>
                    s.ShowId != excludeShowId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AddWithSeatsAsync(
            Show show,
            List<ShowSeat> showSeats)
        {
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                await _context.Shows.AddAsync(show);

                await _context.SaveChangesAsync();

                foreach (var showSeat in showSeats)
                {
                    showSeat.ShowId = show.ShowId;
                }

                await _context.ShowSeats.AddRangeAsync(showSeats);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateAsync(Show show)
        {
            _context.Shows.Update(show);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Show show)
        {
            _context.Shows.Remove(show);

            await _context.SaveChangesAsync();
        }

        public async Task<List<ShowSeat>> GetShowSeatsAsync(
            int showId,
            string? status = null)
        {
            var query = _context.ShowSeats
                .Include(ss => ss.Seat)
                .Where(ss => ss.ShowId == showId);

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(ss => ss.Status == status);
            }

            return await query
                .OrderBy(ss => ss.Seat.RowName)
                .ThenBy(ss => ss.Seat.SeatNumber)
                .ToListAsync();
        }

        public async Task<bool> HasShowSeatsAsync(int showId)
        {
            return await _context.ShowSeats
                .AnyAsync(ss => ss.ShowId == showId);
        }
    }
}