using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.Constants;
using MovieTicketBookingSystem.DTOs.Theatres;
using MovieTicketBookingSystem.Services.Interfaces;

namespace MovieTicketBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TheatresController : ControllerBase
    {
        private readonly ITheatreService _theatreService;

        public TheatresController(ITheatreService theatreService)
        {
            _theatreService = theatreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var theatres = await _theatreService.GetAllAsync(
                pageNumber,
                pageSize);

            return Ok(theatres);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var theatre = await _theatreService.GetByIdAsync(id);

            if (theatre == null)
                return NotFound();

            return Ok(theatre);
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateTheatreRequest request)
        {
            var theatre = await _theatreService.CreateAsync(request);

            return CreatedAtAction(
                nameof(GetById),
                new { id = theatre.TheatreId },
                theatre);
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdateTheatreRequest request)
        {
            var theatre = await _theatreService.UpdateAsync(id, request);

            if (theatre == null)
                return NotFound();

            return Ok(theatre);
        }


        [Authorize(Roles = UserRoles.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _theatreService.DeleteAsync(id);

            if (!isDeleted)
                return NotFound();

            return NoContent();
        }
    }
}