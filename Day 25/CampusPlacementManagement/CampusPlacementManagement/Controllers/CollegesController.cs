using CampusPlacementManagement.DTOs.Colleges;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacementManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CollegesController : ControllerBase
    {
        private readonly ICollegeService collegeService;

        public CollegesController(ICollegeService collegeService)
        {
            this.collegeService = collegeService;
        }

        // POST: api/colleges/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<CollegeResponseDto>> Register(
            RegisterCollegeRequestDto request)
        {
            try
            {
                var result = await collegeService.RegisterAsync(request);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = result.CollegeId },
                    result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // GET: api/colleges
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<ActionResult<List<CollegeResponseDto>>> GetAll()
        {
            var result = await collegeService.GetAllAsync();

            return Ok(result);
        }

        // GET: api/colleges/1
        [Authorize(Roles = "SuperAdmin,PlacementOfficer")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CollegeResponseDto>> GetById(int id)
        {
            var result = await collegeService.GetByIdAsync(id);

            if (result == null)
                return NotFound(new { message = "College not found." });

            return Ok(result);
        }

        // GET: api/colleges/pending
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("pending")]
        public async Task<ActionResult<List<CollegeResponseDto>>> GetPending()
        {
            var result = await collegeService.GetPendingAsync();

            return Ok(result);
        }

        // PUT: api/colleges/1
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CollegeResponseDto>> Update(
            int id,
            UpdateCollegeRequestDto request)
        {
            try
            {
                var result = await collegeService.UpdateAsync(id, request);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/colleges/1/approve
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}/approve")]
        public async Task<ActionResult<CollegeResponseDto>> Approve(int id)
        {
            try
            {
                var result = await collegeService.ApproveAsync(id);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/colleges/1/reject
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}/reject")]
        public async Task<ActionResult<CollegeResponseDto>> Reject(int id)
        {
            try
            {
                var result = await collegeService.RejectAsync(id);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        // PUT: api/colleges/1/suspend
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("{id:int}/suspend")]
        public async Task<ActionResult<CollegeResponseDto>> Suspend(int id)
        {
            try
            {
                var result = await collegeService.SuspendAsync(id);

                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}