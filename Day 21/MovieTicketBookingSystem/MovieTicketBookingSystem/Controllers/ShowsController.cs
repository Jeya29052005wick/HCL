using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Constants;
using MovieTicketBookingSystem.DTOs.Shows;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/shows")]
    public class ShowsController : ControllerBase
    {
        private readonly IShowService _showService;

        public ShowsController(IShowService showService)
        {
            _showService = showService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] DateTime? date,
            [FromQuery] int? movieId,
            [FromQuery] int? theatreId)
        {
            var shows = await _showService.GetAllAsync(
                date, movieId, theatreId);

            return Ok(shows);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByDate(
            [FromQuery] DateTime date)
        {
            var shows = await _showService.GetAllAsync(
                date, null, null);

            return Ok(shows);
        }

        [HttpGet("movie/{movieId:int}")]
        public async Task<IActionResult> GetByMovieId(int movieId)
        {
            var shows = await _showService.GetByMovieIdAsync(movieId);

            return Ok(shows);
        }

        [HttpGet("theatre/{theatreId:int}")]
        public async Task<IActionResult> GetByTheatreId(int theatreId)
        {
            var shows = await _showService.GetByTheatreIdAsync(theatreId);

            return Ok(shows);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var show = await _showService.GetByIdAsync(id);

            if (show == null)
            {
                return NotFound("Show not found.");
            }

            return Ok(show);
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateShowRequest request)
        {
            try
            {
                var show = await _showService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = show.ShowId },
                    show);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "The show could not be created because of a database constraint.");
            }
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateShowRequest request)
        {
            try
            {
                var show = await _showService.UpdateAsync(id, request);

                if (show == null)
                {
                    return NotFound("Show not found.");
                }

                return Ok(show);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "The show could not be updated because of a database constraint.");
            }
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _showService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound("Show not found.");
                }

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "This show cannot be deleted because dependent records exist.");
            }
        }

        [HttpGet("{showId:int}/seats")]
        public async Task<IActionResult> GetShowSeats(int showId)
        {
            var seats = await _showService.GetShowSeatsAsync(showId);

            if (seats == null)
            {
                return NotFound("Show not found.");
            }

            return Ok(seats);
        }

        [HttpGet("{showId:int}/seats/available")]
        public async Task<IActionResult> GetAvailableSeats(int showId)
        {
            var seats = await _showService.GetShowSeatsAsync(
                showId, "Available");

            if (seats == null)
            {
                return NotFound("Show not found.");
            }

            return Ok(seats);
        }

        [HttpGet("{showId:int}/seats/booked")]
        public async Task<IActionResult> GetBookedSeats(int showId)
        {
            var seats = await _showService.GetShowSeatsAsync(
                showId, "Booked");

            if (seats == null)
            {
                return NotFound("Show not found.");
            }

            return Ok(seats);
        }
    }
}