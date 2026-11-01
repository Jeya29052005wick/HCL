using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ShowService : IShowService
    {
        private readonly IShowRepository _showRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IScreenRepository _screenRepository;
        private readonly ISeatRepository _seatRepository;

        public ShowService(
            IShowRepository showRepository,
            IMovieRepository movieRepository,
            IScreenRepository screenRepository,
            ISeatRepository seatRepository)
        {
            _showRepository = showRepository;
            _movieRepository = movieRepository;
            _screenRepository = screenRepository;
            _seatRepository = seatRepository;
        }

        public async Task<List<ShowResponse>> GetAllAsync(
            DateTime? date,
            int? movieId,
            int? theatreId)
        {
            var shows = await _showRepository.GetAllAsync(
                date, movieId, theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<ShowResponse?> GetByIdAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            return show == null ? null : MapToResponse(show);
        }

        public async Task<List<ShowResponse>> GetByMovieIdAsync(
            int movieId)
        {
            var shows = await _showRepository.GetByMovieIdAsync(movieId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<List<ShowResponse>> GetByTheatreIdAsync(
            int theatreId)
        {
            var shows = await _showRepository.GetByTheatreIdAsync(theatreId);

            return shows.Select(MapToResponse).ToList();
        }

        public async Task<ShowResponse> CreateAsync(
            CreateShowRequest request)
        {
            if (request.EndTime <= request.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            if (request.TicketPrice < 0)
            {
                throw new InvalidOperationException(
                    "Ticket price cannot be negative.");
            }

            var movie = await _movieRepository.GetByIdAsync(request.MovieId);

            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

            if (screen == null)
            {
                throw new KeyNotFoundException("Screen not found.");
            }

            var hasOverlap = await _showRepository.HasOverlapAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "The selected screen already has a show scheduled during this time.");
            }

            var physicalSeats =
                await _seatRepository.GetByScreenIdAsync(request.ScreenId);

            if (physicalSeats.Count == 0)
            {
                throw new InvalidOperationException(
                    "The selected screen does not have any seats configured.");
            }

            var show = new Show
            {
                MovieId = request.MovieId,
                ScreenId = request.ScreenId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                TicketPrice = request.TicketPrice
            };

            var showSeats = physicalSeats.Select(seat => new ShowSeat
            {
                SeatId = seat.SeatId,
                Status = "Available"
            }).ToList();

            await _showRepository.AddWithSeatsAsync(show, showSeats);

            var createdShow =
                await _showRepository.GetByIdAsync(show.ShowId);

            return MapToResponse(createdShow!);
        }

        public async Task<ShowResponse?> UpdateAsync(
            int showId,
            UpdateShowRequest request)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            if (show == null)
            {
                return null;
            }

            if (request.EndTime <= request.StartTime)
            {
                throw new InvalidOperationException(
                    "End time must be later than start time.");
            }

            if (request.TicketPrice < 0)
            {
                throw new InvalidOperationException(
                    "Ticket price cannot be negative.");
            }

            var movie = await _movieRepository.GetByIdAsync(request.MovieId);

            if (movie == null)
            {
                throw new KeyNotFoundException("Movie not found.");
            }

            var screen = await _screenRepository.GetByIdAsync(request.ScreenId);

            if (screen == null)
            {
                throw new KeyNotFoundException("Screen not found.");
            }

            if (show.ScreenId != request.ScreenId)
            {
                if (await _showRepository.HasShowSeatsAsync(showId))
                {
                    throw new InvalidOperationException(
                        "The screen cannot be changed after show seats have been generated.");
                }
            }

            var hasOverlap = await _showRepository.HasOverlapAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime,
                showId);

            if (hasOverlap)
            {
                throw new InvalidOperationException(
                    "The selected screen already has a show scheduled during this time.");
            }

            show.MovieId = request.MovieId;
            show.ScreenId = request.ScreenId;
            show.StartTime = request.StartTime;
            show.EndTime = request.EndTime;
            show.TicketPrice = request.TicketPrice;

            await _showRepository.UpdateAsync(show);

            var updatedShow =
                await _showRepository.GetByIdAsync(showId);

            return MapToResponse(updatedShow!);
        }

        public async Task<bool> DeleteAsync(int showId)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            if (show == null)
            {
                return false;
            }

            await _showRepository.DeleteAsync(show);

            return true;
        }

        public async Task<List<ShowSeatResponse>?> GetShowSeatsAsync(
            int showId,
            string? status = null)
        {
            var show = await _showRepository.GetByIdAsync(showId);

            if (show == null)
            {
                return null;
            }

            var showSeats = await _showRepository.GetShowSeatsAsync(
                showId, status);

            return showSeats.Select(ss => new ShowSeatResponse
            {
                SeatId = ss.SeatId,
                RowName = ss.Seat.RowName,
                SeatNumber = ss.Seat.SeatNumber,
                SeatLabel = $"{ss.Seat.RowName}{ss.Seat.SeatNumber}",
                SeatType = ss.Seat.SeatType,
                Price = show.TicketPrice,
                Status = ss.Status
            }).ToList();
        }

        private static ShowResponse MapToResponse(Show show)
        {
            return new ShowResponse
            {
                ShowId = show.ShowId,
                MovieId = show.MovieId,
                MovieTitle = show.Movie.Title,
                ScreenId = show.ScreenId,
                ScreenName = show.Screen.ScreenName,
                TheatreId = show.Screen.TheatreId,
                TheatreName = show.Screen.Theatre.Name,
                StartTime = show.StartTime,
                EndTime = show.EndTime,
                TicketPrice = show.TicketPrice
            };
        }
    }
}