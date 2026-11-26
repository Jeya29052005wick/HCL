using HospitalAppointment.DTOs;

namespace HospitalAppointment.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> RegisterAsync(RegisterRequest request);
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}