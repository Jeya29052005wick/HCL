using CampusPlacementManagement.Data;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CampusPlacementManagement.Repositories
{
    public class CollegeRepository : ICollegeRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CollegeRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<College?> GetByIdAsync(int collegeId)
        {
            return await dbContext.Colleges
                .FirstOrDefaultAsync(c => c.CollegeId == collegeId);
        }

        public async Task<List<College>> GetAllAsync()
        {
            return await dbContext.Colleges
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<College>> GetPendingAsync()
        {
            return await dbContext.Colleges
                .AsNoTracking()
                .Where(c => c.Status == CollegeStatus.Pending)
                .ToListAsync();
        }

        public async Task<bool> ExistsByCollegeCodeAsync(string collegeCode)
        {
            return await dbContext.Colleges
                .AnyAsync(c => c.CollegeCode == collegeCode);
        }

        public async Task AddAsync(College college)
        {
            await dbContext.Colleges.AddAsync(college);
        }

        public void Update(College college)
        {
            dbContext.Colleges.Update(college);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}