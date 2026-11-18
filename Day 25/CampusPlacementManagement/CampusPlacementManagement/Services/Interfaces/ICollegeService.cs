using CampusPlacementManagement.DTOs.Colleges;

namespace CampusPlacementManagement.Services.Interfaces
{
    public interface ICollegeService
    {
        Task<CollegeResponseDto> RegisterAsync(RegisterCollegeRequestDto request);

        Task<CollegeResponseDto?> GetByIdAsync(int collegeId);

        Task<List<CollegeResponseDto>> GetAllAsync();

        Task<List<CollegeResponseDto>> GetPendingAsync();

        Task<CollegeResponseDto> UpdateAsync(
            int collegeId,
            UpdateCollegeRequestDto request);

        Task<CollegeResponseDto> ApproveAsync(int collegeId);

        Task<CollegeResponseDto> RejectAsync(int collegeId);

        Task<CollegeResponseDto> SuspendAsync(int collegeId);
    }
}