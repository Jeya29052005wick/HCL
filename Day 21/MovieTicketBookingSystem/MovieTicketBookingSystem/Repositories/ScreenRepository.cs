using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Data;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;

namespace MovieTicketBookingSystem.Repositories
{
    public class ScreenRepository : IScreenRepository
    {
        private readonly ApplicationDbContext _context;

        public ScreenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Screen>> GetAllAsync(int? theatreId)
        {
            var query = _context.Screens.AsQueryable();

            if (theatreId.HasValue)
            {
                query = query.Where(s => s.TheatreId == theatreId.Value);
            }

            return await query
                .OrderBy(s => s.ScreenId)
                .ToListAsync();
        }

        public async Task<Screen?> GetByIdAsync(int screenId)
        {
            return await _context.Screens
                .FirstOrDefaultAsync(s => s.ScreenId == screenId);
        }

        public async Task<List<Screen>> GetByTheatreIdAsync(int theatreId)
        {
            return await _context.Screens
                .Where(s => s.TheatreId == theatreId)
                .OrderBy(s => s.ScreenId)
                .ToListAsync();
        }

        public async Task AddAsync(Screen screen)
        {
            await _context.Screens.AddAsync(screen);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Screen screen)
        {
            _context.Screens.Update(screen);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Screen screen)
        {
            _context.Screens.Remove(screen);
            await _context.SaveChangesAsync();
        }
    }
}