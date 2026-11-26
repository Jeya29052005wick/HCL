using HospitalAppointment.DTOs;
using HospitalAppointment.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HospitalAppointment.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> BookAppointment(CreateAppointmentRequest request)
    {
        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _appointmentService.BookAppointmentAsync(patientId, request);

        return Ok(result);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetMyAppointments()
    {
        var patientId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await _appointmentService.GetMyAppointmentsAsync(patientId);

        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetAllAppointments()
    {
        var result = await _appointmentService.GetAllAppointmentsAsync();

        return Ok(result);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> ApproveAppointment(int id)
    {
        var result = await _appointmentService.ApproveAppointmentAsync(id);

        if (!result)
            return BadRequest(new { message = "Appointment not found or already processed." });

        return Ok(new { message = "Appointment booked successfully." });
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> RejectAppointment(int id)
    {
        var result = await _appointmentService.RejectAppointmentAsync(id);

        if (!result)
            return BadRequest(new { message = "Appointment not found or already processed." });

        return Ok(new { message = "Appointment cancelled." });
    }
}