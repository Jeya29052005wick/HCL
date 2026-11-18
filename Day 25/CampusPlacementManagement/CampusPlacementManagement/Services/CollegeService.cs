using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Enums;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services
{
    public class CollegeService : ICollegeService
    {
        private readonly ICollegeRepository collegeRepository;

        public CollegeService(ICollegeRepository collegeRepository)
        {
            this.collegeRepository = collegeRepository;
        }

        public async Task<CollegeResponseDto> RegisterAsync(
            RegisterCollegeRequestDto request)
        {
            bool exists = await collegeRepository
                .ExistsByCollegeCodeAsync(request.CollegeCode);

            if (exists)
                throw new InvalidOperationException("College code already exists.");

            var college = new College
            {
                CollegeName = request.CollegeName,
                CollegeCode = request.CollegeCode,
                UniversityName = request.UniversityName,
                OfficialEmail = request.OfficialEmail,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Website = request.Website,
                Status = CollegeStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await collegeRepository.AddAsync(college);
            await collegeRepository.SaveChangesAsync();

            return MapToResponse(college);
        }

        public async Task<CollegeResponseDto?> GetByIdAsync(int collegeId)
        {
            var college = await collegeRepository.GetByIdAsync(collegeId);

            return college == null ? null : MapToResponse(college);
        }

        public async Task<List<CollegeResponseDto>> GetAllAsync()
        {
            var colleges = await collegeRepository.GetAllAsync();

            return colleges.Select(MapToResponse).ToList();
        }

        public async Task<List<CollegeResponseDto>> GetPendingAsync()
        {
            var colleges = await collegeRepository.GetPendingAsync();

            return colleges.Select(MapToResponse).ToList();
        }

        public async Task<CollegeResponseDto> UpdateAsync(
            int collegeId,
            UpdateCollegeRequestDto request)
        {
            var college = await collegeRepository.GetByIdAsync(collegeId);

            if (college == null)
                throw new KeyNotFoundException("College not found.");

            if (college.CollegeCode != request.CollegeCode)
            {
                bool exists = await collegeRepository
                    .ExistsByCollegeCodeAsync(request.CollegeCode);

                if (exists)
                    throw new InvalidOperationException("College code already exists.");
            }

            college.CollegeName = request.CollegeName;
            college.CollegeCode = request.CollegeCode;
            college.UniversityName = request.UniversityName;
            college.OfficialEmail = request.OfficialEmail;
            college.PhoneNumber = request.PhoneNumber;
            college.Address = request.Address;
            college.Website = request.Website;

            collegeRepository.Update(college);
            await collegeRepository.SaveChangesAsync();

            return MapToResponse(college);
        }

        public async Task<CollegeResponseDto> ApproveAsync(int collegeId)
        {
            var college = await GetCollegeOrThrowAsync(collegeId);

            if (college.Status != CollegeStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending colleges can be approved.");

            college.Status = CollegeStatus.Approved;

            collegeRepository.Update(college);
            await collegeRepository.SaveChangesAsync();

            return MapToResponse(college);
        }

        public async Task<CollegeResponseDto> RejectAsync(int collegeId)
        {
            var college = await GetCollegeOrThrowAsync(collegeId);

            if (college.Status != CollegeStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending colleges can be rejected.");

            college.Status = CollegeStatus.Rejected;

            collegeRepository.Update(college);
            await collegeRepository.SaveChangesAsync();

            return MapToResponse(college);
        }

        public async Task<CollegeResponseDto> SuspendAsync(int collegeId)
        {
            var college = await GetCollegeOrThrowAsync(collegeId);

            if (college.Status != CollegeStatus.Approved)
                throw new InvalidOperationException(
                    "Only approved colleges can be suspended.");

            college.Status = CollegeStatus.Suspended;

            collegeRepository.Update(college);
            await collegeRepository.SaveChangesAsync();

            return MapToResponse(college);
        }

        private async Task<College> GetCollegeOrThrowAsync(int collegeId)
        {
            var college = await collegeRepository.GetByIdAsync(collegeId);

            if (college == null)
                throw new KeyNotFoundException("College not found.");

            return college;
        }

        private static CollegeResponseDto MapToResponse(College college)
        {
            return new CollegeResponseDto
            {
                CollegeId = college.CollegeId,
                CollegeName = college.CollegeName,
                CollegeCode = college.CollegeCode,
                UniversityName = college.UniversityName,
                OfficialEmail = college.OfficialEmail,
                PhoneNumber = college.PhoneNumber,
                Address = college.Address,
                Website = college.Website,
                Status = college.Status.ToString(),
                CreatedAt = college.CreatedAt
            };
        }
    }
}