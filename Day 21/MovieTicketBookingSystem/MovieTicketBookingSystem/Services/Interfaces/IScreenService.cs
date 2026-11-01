using MovieTicketBookingSystem.DTOs.Screens;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IScreenService
    {
        Task<List<ScreenResponse>> GetAllAsync(int? theatreId);

        Task<ScreenResponse?> GetByIdAsync(int screenId);

        Task<List<ScreenResponse>?> GetByTheatreIdAsync(int theatreId);

        Task<ScreenResponse?> CreateAsync(CreateScreenRequest request);

        Task<ScreenResponse?> UpdateAsync(
            int screenId,
            UpdateScreenRequest request);

        Task<bool> DeleteAsync(int screenId);
    }
}