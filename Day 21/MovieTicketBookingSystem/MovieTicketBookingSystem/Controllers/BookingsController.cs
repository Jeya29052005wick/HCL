
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.Authorization;
using MovieTicketBookingSystem.DTOs.Bookings;
using MovieTicketBookingSystem.Models;
using MovieTicketBookingSystem.Services.Interfaces;
using System.Security.Claims;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IAuthorizationService _authorizationService;

        public BookingsController(
            IBookingService bookingService,
            IAuthorizationService authorizationService)
        {
            _bookingService = bookingService;
            _authorizationService = authorizationService;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var booking = await _bookingService.GetByIdAsync(id);

            if (booking == null)
            {
                return NotFound(new
                {
                    message = "Booking not found."
                });
            }

            return Ok(booking);
        }

        [AllowAnonymous]
        [HttpGet("show/{showId:int}")]
        public async Task<IActionResult> GetByShowId(int showId)
        {
            var bookings =
                await _bookingService.GetByShowIdAsync(showId);

            return Ok(bookings);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(
            CreateBookingRequest request)
        {
            try
            {
                var userIdClaim =
                    User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new
                    {
                        message = "Invalid or missing user ID in token."
                    });
                }

                var booking =
                    await _bookingService.CreateAsync(request, userId);

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

        [Authorize]
        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                // 1. Retrieve the actual booking entity.
                var booking =
                    await _bookingService.GetEntityByIdAsync(id);

                if (booking == null)
                {
                    return NotFound(new
                    {
                        message = "Booking not found."
                    });
                }

                // 2. Check whether the logged-in user owns this booking.
                var authorizationResult =
                    await _authorizationService.AuthorizeAsync(
                        User,
                        booking,
                        new BookingOwnerRequirement());

                // 3. Reject users who do not own this booking.
                if (!authorizationResult.Succeeded)
                {
                    return Forbid();
                }

                // 4. Cancel the booking after authorization succeeds.
                var cancelledBooking =
                    await _bookingService.CancelAsync(id);

                if (cancelledBooking == null)
                {
                    return NotFound(new
                    {
                        message = "Booking not found."
                    });
                }

                return Ok(cancelledBooking);
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
