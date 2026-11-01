using Microsoft.AspNetCore.Mvc;
using MovieTicketBookingSystem.DTOs.Movies;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null)
        {
            var movies = await _movieService.GetAllAsync(
                pageNumber,
                pageSize,
                title
            );

            return Ok(movies);
        }

        // GET: api/movies/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetByIdAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // POST: api/movies
        [HttpPost]
        public async Task<IActionResult> Create(CreateMovieRequest request)
        {
            var movie = await _movieService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = movie.MovieId },
                movie
            );
        }

        // PUT: api/movies/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateMovieRequest request)
        {
            var movie = await _movieService.UpdateAsync(id, request);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // DELETE: api/movies/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _movieService.DeleteAsync(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}