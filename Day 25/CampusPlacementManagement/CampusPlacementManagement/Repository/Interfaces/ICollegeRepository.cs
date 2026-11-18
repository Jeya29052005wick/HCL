using CampusPlacementManagement.Models;

namespace CampusPlacementManagement.Repositories.Interfaces
{
    public interface ICollegeRepository
    {
        Task<College?> GetByIdAsync(int collegeId);

        Task<List<College>> GetAllAsync();

        Task<List<College>> GetPendingAsync();

        Task<bool> ExistsByCollegeCodeAsync(string collegeCode);

        Task AddAsync(College college);

        void Update(College college);

        Task SaveChangesAsync();
    }
}