using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? showId,
            [FromQuery] string? status)
        {
            var bookings = await _bookingService.GetAllAsync(
                showId,
                status);

            return Ok(bookings);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound(new { message = "Booking not found." });
            }

            return Ok(booking);
        }

        [HttpGet("show/{showId:int}")]
        public async Task<IActionResult> GetByShowId(int showId)
        {
            var bookings =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateBookingRequest request)
        {
            try
            {
                var booking = await _bookingService.CreateAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = booking.BookingId },
                    booking);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "The booking could not be completed due to a database conflict."
                });
            }
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                var booking = await _bookingService.CancelAsync(id);

                if (booking == null)
                {
                    return NotFound(new { message = "Booking not found." });
                }

                return Ok(booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "The booking could not be cancelled."
                });
            }
        }
    }
}