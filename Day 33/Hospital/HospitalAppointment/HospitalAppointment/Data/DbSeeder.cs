using HospitalAppointment.Enums;
using HospitalAppointment.Models;

namespace HospitalAppointment.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Users.Any(u => u.Role == UserRole.Doctor))
            return;

        var doctor = new User
        {
            Name = "Default Doctor",
            Email = "doctor@hospital.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Doctor@123"),
            Role = UserRole.Doctor
        };

        context.Users.Add(doctor);

        await context.SaveChangesAsync();
    }
}