using MovieTicketBookingSystem.DTOs.Theatres;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface ITheatreService
    {
        Task<List<TheatreResponse>> GetAllAsync(
            int pageNumber,
            int pageSize);

        Task<TheatreResponse?> GetByIdAsync(int theatreId);

        Task<TheatreResponse> CreateAsync(CreateTheatreRequest request);

        Task<TheatreResponse?> UpdateAsync(
            int theatreId,
            UpdateTheatreRequest request);

        Task<bool> DeleteAsync(int theatreId);
    }
}