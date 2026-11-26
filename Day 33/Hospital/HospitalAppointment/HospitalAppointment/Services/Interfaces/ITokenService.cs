using HospitalAppointment.Models;

namespace HospitalAppointment.Services.Interfaces;

public interface ITokenService
{
    string GenerateToken(User user);
}