using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBookingSystem.DTOs.Screens;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScreensController : ControllerBase
    {
        private readonly IScreenService _screenService;

        public ScreensController(IScreenService screenService)
        {
            _screenService = screenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int? theatreId = null)
        {
            var screens = await _screenService.GetAllAsync(theatreId);

            return Ok(screens);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var screen = await _screenService.GetByIdAsync(id);

            if (screen == null)
                return NotFound();

            return Ok(screen);
        }

        [HttpGet("theatre/{theatreId:int}")]
        public async Task<IActionResult> GetByTheatreId(int theatreId)
        {
            var screens = await _screenService.GetByTheatreIdAsync(theatreId);

            if (screens == null)
                return NotFound();

            return Ok(screens);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateScreenRequest request)
        {
            try
            {
                var screen = await _screenService.CreateAsync(request);

                if (screen == null)
                    return NotFound("The specified theatre does not exist.");

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = screen.ScreenId },
                    screen);
            }
            catch (DbUpdateException)
            {
                return Conflict("A screen with this name already exists in this theatre.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateScreenRequest request)
        {
            try
            {
                var screen = await _screenService.UpdateAsync(id, request);

                if (screen == null)
                    return NotFound();

                return Ok(screen);
            }
            catch (DbUpdateException)
            {
                return Conflict("A screen with this name already exists in this theatre.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var isDeleted = await _screenService.DeleteAsync(id);

                if (!isDeleted)
                    return NotFound();

                return NoContent();
            }
            catch (DbUpdateException)
            {
                return Conflict("This screen cannot be deleted because dependent records exist.");
            }
        }
    }
}