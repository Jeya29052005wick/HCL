using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IScreenRepository _screenRepository;

        public SeatService(
            ISeatRepository seatRepository,
            IScreenRepository screenRepository)
        {
            _seatRepository = seatRepository;
            _screenRepository = screenRepository;
        }

        public async Task<List<SeatResponse>?> GetByScreenIdAsync(
            int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return null;

            var seats = await _seatRepository.GetByScreenIdAsync(screenId);

            return seats.Select(s => new SeatResponse
            {
                SeatId = s.SeatId,
                ScreenId = s.ScreenId,
                RowName = s.RowName,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType
            }).ToList();
        }

        public async Task<SeatResponse?> GetByIdAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return null;

            return MapToResponse(seat);
        }

        public async Task<SeatResponse?> CreateAsync(
            CreateSeatRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(
                request.ScreenId);

            if (screen == null)
                return null;

            var exists = await _seatRepository.ExistsAsync(
                request.ScreenId,
                request.RowName,
                request.SeatNumber);

            if (exists)
                throw new InvalidOperationException(
                    "This seat coordinate already exists in the screen.");

            var seat = new Seat
            {
                ScreenId = request.ScreenId,
                RowName = request.RowName,
                SeatNumber = request.SeatNumber,
                SeatType = request.SeatType
            };

            await _seatRepository.AddAsync(seat);

            return MapToResponse(seat);
        }

        public async Task<List<SeatResponse>?> BulkCreateAsync(
            BulkCreateSeatsRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(
                request.ScreenId);

            if (screen == null)
                return null;

            var requestedCoordinates = request.Seats
                .Select(s => new
                {
                    RowName = s.RowName,
                    s.SeatNumber
                })
                .ToList();

            var duplicateInRequest = requestedCoordinates
                .GroupBy(s => new { s.RowName, s.SeatNumber })
                .Any(g => g.Count() > 1);

            if (duplicateInRequest)
                throw new InvalidOperationException(
                    "The bulk request contains duplicate seat coordinates.");

            var existingSeats = await _seatRepository.GetByScreenIdAsync(
                request.ScreenId);

            var existingCoordinates = existingSeats
                .Select(s => new
                {
                    s.RowName,
                    s.SeatNumber
                })
                .ToHashSet();

            var conflicts = requestedCoordinates.Any(s =>
                existingCoordinates.Contains(new
                {
                    s.RowName,
                    s.SeatNumber
                }));

            if (conflicts)
                throw new InvalidOperationException(
                    "One or more seat coordinates already exist in this screen.");

            var seats = request.Seats.Select(s => new Seat
            {
                ScreenId = request.ScreenId,
                RowName = s.RowName,
                SeatNumber = s.SeatNumber,
                SeatType = s.SeatType
            }).ToList();

            await _seatRepository.AddRangeAsync(seats);

            return seats.Select(MapToResponse).ToList();
        }

        public async Task<SeatResponse?> UpdateAsync(
            int seatId,
            UpdateSeatRequest request)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return null;

            var screen = await _screenRepository.GetByIdAsync(
                request.ScreenId);

            if (screen == null)
                return null;

            var exists = await _seatRepository.ExistsAsync(
                request.ScreenId,
                request.RowName,
                request.SeatNumber,
                seatId);

            if (exists)
                throw new InvalidOperationException(
                    "This seat coordinate already exists in the screen.");

            seat.ScreenId = request.ScreenId;
            seat.RowName = request.RowName;
            seat.SeatNumber = request.SeatNumber;
            seat.SeatType = request.SeatType;

            await _seatRepository.UpdateAsync(seat);

            return MapToResponse(seat);
        }

        public async Task<bool> DeleteAsync(int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);

            if (seat == null)
                return false;

            await _seatRepository.DeleteAsync(seat);

            return true;
        }

        private static SeatResponse MapToResponse(Seat seat)
        {
            return new SeatResponse
            {
                SeatId = seat.SeatId,
                ScreenId = seat.ScreenId,
                RowName = seat.RowName,
                SeatNumber = seat.SeatNumber,
                SeatType = seat.SeatType
            };
        }
    }
}