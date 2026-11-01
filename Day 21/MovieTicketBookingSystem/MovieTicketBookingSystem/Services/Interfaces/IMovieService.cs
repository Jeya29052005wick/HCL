using MovieTicketBookingSystem.DTOs.Movies;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IMovieService
    {
        Task<List<MovieResponse>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title
        );

        Task<MovieResponse?> GetByIdAsync(int movieId);

        Task<MovieResponse> CreateAsync(CreateMovieRequest request);

        Task<MovieResponse?> UpdateAsync(
            int movieId,
            UpdateMovieRequest request
        );

        Task<bool> DeleteAsync(int movieId);
    }
}