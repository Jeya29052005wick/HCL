using MovieTicketBookingSystem.DTOs.Shows;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IShowService
    {
        Task<List<ShowResponse>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId);

        Task<ShowResponse?> GetByIdAsync(int showId);

        Task<List<ShowResponse>> GetByMovieIdAsync(int movieId);

        Task<List<ShowResponse>> GetByTheatreIdAsync(int theatreId);

        Task<ShowResponse> CreateAsync(CreateShowRequest request);

        Task<ShowResponse?> UpdateAsync(
            int showId,
            UpdateShowRequest request);

        Task<bool> DeleteAsync(int showId);

        Task<List<ShowSeatResponse>?> GetShowSeatsAsync(
            int showId,
            string? status = null);
    }
}