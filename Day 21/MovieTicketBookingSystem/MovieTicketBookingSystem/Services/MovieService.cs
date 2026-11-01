using MovieTicketBookingSystem.DTOs.Movies;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Repositories.Interfaces;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MovieResponse>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? title)
        {
            var movies = await _movieRepository.GetAllAsync(
                pageNumber,
                pageSize,
                title
            );

            return movies.Select(movie => new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            }).ToList();
        }

        public async Task<MovieResponse?> GetByIdAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                return null;
            }

            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }

        public async Task<MovieResponse> CreateAsync(CreateMovieRequest request)
        {
            var movie = new Movie
            {
                Title = request.Title,
                Description = request.Description,
                DurationMinutes = request.DurationMinutes,
                ReleaseDate = request.ReleaseDate,
                Language = request.Language,
                Genre = request.Genre
            };

            await _movieRepository.AddAsync(movie);

            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }

        public async Task<MovieResponse?> UpdateAsync(
            int movieId,
            UpdateMovieRequest request)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                return null;
            }

            movie.Title = request.Title;
            movie.Description = request.Description;
            movie.DurationMinutes = request.DurationMinutes;
            movie.ReleaseDate = request.ReleaseDate;
            movie.Language = request.Language;
            movie.Genre = request.Genre;

            await _movieRepository.UpdateAsync(movie);

            return new MovieResponse
            {
                MovieId = movie.MovieId,
                Title = movie.Title,
                Description = movie.Description,
                DurationMinutes = movie.DurationMinutes,
                ReleaseDate = movie.ReleaseDate,
                Language = movie.Language,
                Genre = movie.Genre
            };
        }

        public async Task<bool> DeleteAsync(int movieId)
        {
            var movie = await _movieRepository.GetByIdAsync(movieId);

            if (movie == null)
            {
                return false;
            }

            await _movieRepository.DeleteAsync(movie);

            return true;
        }
    }
}