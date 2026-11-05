using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<List<BookingResponse>> GetAllAsync(
            int? showId,
            string? status);

        Task<BookingResponse?> GetByIdAsync(int bookingId);

        Task<List<BookingResponse>> GetByShowIdAsync(int showId);

        Task<BookingResponse> CreateAsync(
    CreateBookingRequest request,
    int userId);

        Task<BookingResponse?> CancelAsync(int bookingId);

        Task<Booking?> GetEntityByIdAsync(int bookingId);
    }
}