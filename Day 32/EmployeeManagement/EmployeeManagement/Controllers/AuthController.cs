
using EmployeeManagement.DTOs.Auth;
using EmployeeManagement.Models;
using EmployeeManagement.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtService _jwtService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            JwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            // Find user using email
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // Verify password
            bool passwordValid = await _userManager
                .CheckPasswordAsync(user, dto.Password);

            if (!passwordValid)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password."
                });
            }

            // Get user's roles
            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Count == 0)
            {
                return Unauthorized(new
                {
                    message = "No role assigned to this user."
                });
            }

            // Generate JWT token
            string token = _jwtService.GenerateToken(user, roles);

            // Send response
            var response = new LoginResponseDto
            {
                Token = token,
                Email = user.Email!,
                Role = roles[0]
            };

            return Ok(response);
        }
    }
}
