using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Constants;
using MovieTicketBookingSystem.DTOs.Seats;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;

        public SeatsController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        // GET: api/seats/screen/1
        [HttpGet("screen/{screenId:int}")]
        public async Task<IActionResult> GetByScreenId(int screenId)
        {
            var seats = await _seatService.GetByScreenIdAsync(screenId);

            if (seats == null)
                return NotFound("Screen not found.");

            return Ok(seats);
        }

        // GET: api/seats/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound();

            return Ok(seat);
        }

        // POST: api/seats
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.CreateAsync(request);

                if (seat == null)
                    return NotFound("Screen not found.");

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = seat.SeatId },
                    seat);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "The seat could not be created because of a database constraint.");
            }
        }

        // POST: api/seats/bulk
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpPost("bulk")]
        public async Task<IActionResult> BulkCreate(
            BulkCreateSeatsRequest request)
        {
            try
            {
                var seats = await _seatService.BulkCreateAsync(request);

                if (seats == null)
                    return NotFound("Screen not found.");

                return StatusCode(
                    StatusCodes.Status201Created,
                    seats);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "One or more seats could not be created because of a database constraint.");
            }
        }

        // PUT: api/seats/1
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateSeatRequest request)
        {
            try
            {
                var seat = await _seatService.UpdateAsync(id, request);

                if (seat == null)
                    return NotFound(
                        "Seat or specified screen not found.");

                return Ok(seat);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "The seat could not be updated because of a database constraint.");
            }
        }

        // DELETE: api/seats/1
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.TheatreManager)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _seatService.DeleteAsync(id);

                if (!isDeleted)
                    return NotFound();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict(
                    "This seat cannot be deleted because dependent records exist.");
            }
        }
    }
}