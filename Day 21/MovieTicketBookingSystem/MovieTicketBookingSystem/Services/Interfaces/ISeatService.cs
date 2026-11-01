using MovieTicketBookingSystem.DTOs.Seats;

namespace MovieTicketBookingSystem.Services.Interfaces
{
    public interface ISeatService
    {
        Task<List<SeatResponse>?> GetByScreenIdAsync(int screenId);

        Task<SeatResponse?> GetByIdAsync(int seatId);

        Task<SeatResponse?> CreateAsync(CreateSeatRequest request);

        Task<List<SeatResponse>?> BulkCreateAsync(
            BulkCreateSeatsRequest request);

        Task<SeatResponse?> UpdateAsync(
            int seatId,
            UpdateSeatRequest request);

        Task<bool> DeleteAsync(int seatId);
    }
}