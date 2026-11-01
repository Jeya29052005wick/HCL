using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IShowRepository
    {
        Task<List<Show>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId);

        Task<Show?> GetByIdAsync(int showId);

        Task<List<Show>> GetByMovieIdAsync(int movieId);

        Task<List<Show>> GetByTheatreIdAsync(int theatreId);

        Task<bool> HasOverlapAsync(
            int screenId,
            DateTime startTime,
            DateTime endTime,
            int? excludeShowId = null);

        Task AddWithSeatsAsync(
            Show show,
            List<ShowSeat> showSeats);

        Task UpdateAsync(Show show);

        Task DeleteAsync(Show show);

        Task<List<ShowSeat>> GetShowSeatsAsync(
            int showId,
            string? status = null);

        Task<bool> HasShowSeatsAsync(int showId);
    }
}