using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;
        private readonly ITheatreRepository _theatreRepository;

        public ScreenService(
            IScreenRepository screenRepository,
            ITheatreRepository theatreRepository)
        {
            _screenRepository = screenRepository;
            _theatreRepository = theatreRepository;
        }

        public async Task<List<ScreenResponse>> GetAllAsync(int? theatreId)
        {
            var screens = await _screenRepository.GetAllAsync(theatreId);

            return screens.Select(s => new ScreenResponse
            {
                ScreenId = s.ScreenId,
                TheatreId = s.TheatreId,
                ScreenName = s.ScreenName
            }).ToList();
        }

        public async Task<ScreenResponse?> GetByIdAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return null;

            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }

        public async Task<List<ScreenResponse>?> GetByTheatreIdAsync(
            int theatreId)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);

            if (theatre == null)
                return null;

            var screens = await _screenRepository.GetByTheatreIdAsync(theatreId);

            return screens.Select(s => new ScreenResponse
            {
                ScreenId = s.ScreenId,
                TheatreId = s.TheatreId,
                ScreenName = s.ScreenName
            }).ToList();
        }

        public async Task<ScreenResponse?> CreateAsync(
            CreateScreenRequest request)
        {
            var theatre = await _theatreRepository.GetByIdAsync(
                request.TheatreId);

            if (theatre == null)
                return null;

            var screen = new Screen
            {
                TheatreId = request.TheatreId,
                ScreenName = request.ScreenName
            };

            await _screenRepository.AddAsync(screen);

            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }

        public async Task<ScreenResponse?> UpdateAsync(
            int screenId,
            UpdateScreenRequest request)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return null;

            var theatre = await _theatreRepository.GetByIdAsync(
                request.TheatreId);

            if (theatre == null)
                return null;

            screen.TheatreId = request.TheatreId;
            screen.ScreenName = request.ScreenName;

            await _screenRepository.UpdateAsync(screen);

            return new ScreenResponse
            {
                ScreenId = screen.ScreenId,
                TheatreId = screen.TheatreId,
                ScreenName = screen.ScreenName
            };
        }

        public async Task<bool> DeleteAsync(int screenId)
        {
            var screen = await _screenRepository.GetByIdAsync(screenId);

            if (screen == null)
                return false;

            await _screenRepository.DeleteAsync(screen);

            return true;
        }
    }
}