using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IShowRepository _showRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IShowRepository showRepository)
        {
            _bookingRepository = bookingRepository;
            _showRepository = showRepository;
        }

        public async Task<List<BookingResponse>> GetAllAsync(
            int? showId,
            string? status)
        {
            var bookings = await _bookingRepository.GetAllAsync(
                showId,
                status);

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<BookingResponse?> GetByIdAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);

            return booking == null ? null : MapToResponse(booking);
        }

        public async Task<List<BookingResponse>> GetByShowIdAsync(int showId)
        {
            var bookings = await _bookingRepository.GetByShowIdAsync(showId);

            return bookings.Select(MapToResponse).ToList();
        }

        public async Task<BookingResponse> CreateAsync(
            CreateBookingRequest request)
        {
            // Validate that the show exists.
            var show = await _showRepository.GetByIdAsync(request.ShowId);

            if (show == null)
            {
                throw new KeyNotFoundException("Show not found.");
            }

            // Reject duplicate seat IDs in the request.
            if (request.SeatIds.Distinct().Count() != request.SeatIds.Count)
            {
                throw new InvalidOperationException(
                    "Duplicate seat IDs are not allowed.");
            }

            // Get available seats for this show.
            var availableShowSeats =
                await _showRepository.GetShowSeatsAsync(
                    request.ShowId,
                    "Available");

            var requestedSeatIds = request.SeatIds.ToHashSet();

            // Every requested seat must be available for this show.
            var selectedShowSeats = availableShowSeats
                .Where(ss => requestedSeatIds.Contains(ss.SeatId))
                .ToList();

            if (selectedShowSeats.Count != request.SeatIds.Count)
            {
                throw new InvalidOperationException(
                    "One or more selected seats are invalid or already booked.");
            }

            // Calculate amount using the server-side show price.
            decimal totalAmount =
                selectedShowSeats.Count * show.TicketPrice;

            var booking = new Booking
            {
                BookingReference =
                    "MOV" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),

                ShowId = request.ShowId,
                CustomerName = request.CustomerName.Trim(),
                CustomerEmail = request.CustomerEmail.Trim(),
                CustomerPhone = request.CustomerPhone.Trim(),
                BookingDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = "Confirmed",

                BookingSeats = selectedShowSeats.Select(ss =>
                    new BookingSeat
                    {
                        SeatId = ss.SeatId,
                        Price = show.TicketPrice
                    }).ToList()
            };

            await _bookingRepository.AddWithSeatsAsync(
                booking,
                request.SeatIds);

            var createdBooking =
                await _bookingRepository.GetByIdAsync(booking.BookingId);

            if (createdBooking == null)
            {
                throw new InvalidOperationException(
                    "Booking was created but could not be retrieved.");
            }

            return MapToResponse(createdBooking);
        }

        public async Task<BookingResponse?> CancelAsync(int bookingId)
        {
            var booking =
                await _bookingRepository.GetByIdAsync(bookingId);

            if (booking == null)
            {
                return null;
            }

            if (booking.Status != "Confirmed")
            {
                throw new InvalidOperationException(
                    "Only confirmed bookings can be cancelled.");
            }

            await _bookingRepository.CancelAsync(booking);

            var updatedBooking =
                await _bookingRepository.GetByIdAsync(bookingId);

            return updatedBooking == null
                ? null
                : MapToResponse(updatedBooking);
        }

        private static BookingResponse MapToResponse(Booking booking)
        {
            return new BookingResponse
            {
                BookingId = booking.BookingId,
                BookingReference = booking.BookingReference,
                ShowId = booking.ShowId,
                CustomerName = booking.CustomerName,
                CustomerEmail = booking.CustomerEmail,
                CustomerPhone = booking.CustomerPhone,
                BookingDate = booking.BookingDate,
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,

                Seats = booking.BookingSeats
                    .Select(bs =>
                        $"{bs.Seat.RowName}{bs.Seat.SeatNumber}")
                    .ToList()
            };
        }
    }
}