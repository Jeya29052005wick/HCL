using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class TheatreService : ITheatreService
    {
        private readonly ITheatreRepository _theatreRepository;

        public TheatreService(ITheatreRepository theatreRepository)
        {
            _theatreRepository = theatreRepository;
        }

        public async Task<List<TheatreResponse>> GetAllAsync(
            int pageNumber,
            int pageSize)
        {
            var theatres = await _theatreRepository.GetAllAsync(
                pageNumber,
                pageSize);

            return theatres.Select(t => new TheatreResponse
            {
                TheatreId = t.TheatreId,
                Name = t.Name,
                Location = t.Location
            }).ToList();
        }

        public async Task<TheatreResponse?> GetByIdAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);

            if (theatre == null)
                return null;

            return new TheatreResponse
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Location = theatre.Location
            };
        }

        public async Task<TheatreResponse> CreateAsync(
            CreateTheatreRequest request)
        {
            var theatre = new Theatre
            {
                Name = request.Name,
                Location = request.Location
            };

            await _theatreRepository.AddAsync(theatre);

            return new TheatreResponse
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Location = theatre.Location
            };
        }

        public async Task<TheatreResponse?> UpdateAsync(
            int theatreId,
            UpdateTheatreRequest request)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);

            if (theatre == null)
                return null;

            theatre.Name = request.Name;
            theatre.Location = request.Location;

            await _theatreRepository.UpdateAsync(theatre);

            return new TheatreResponse
            {
                TheatreId = theatre.TheatreId,
                Name = theatre.Name,
                Location = theatre.Location
            };
        }

        public async Task<bool> DeleteAsync(int theatreId)
        {
            var theatre = await _theatreRepository.GetByIdAsync(theatreId);

            if (theatre == null)
                return false;

            await _theatreRepository.DeleteAsync(theatre);

            return true;
        }
    }
}