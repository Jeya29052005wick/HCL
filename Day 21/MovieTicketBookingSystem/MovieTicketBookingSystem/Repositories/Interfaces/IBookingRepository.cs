using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<List<Booking>> GetAllAsync(int? showId, string? status);

        Task<Booking?> GetByIdAsync(int bookingId);

        Task<List<Booking>> GetByShowIdAsync(int showId);

        Task AddWithSeatsAsync(
            Booking booking,
            List<int> seatIds);

        Task CancelAsync(Booking booking);
    }
}