using HospitalAppointment.DTOs;

namespace HospitalAppointment.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponse> BookAppointmentAsync(int patientId, CreateAppointmentRequest request);
    Task<List<AppointmentResponse>> GetMyAppointmentsAsync(int patientId);
    Task<List<AppointmentResponse>> GetAllAppointmentsAsync();
    Task<bool> ApproveAppointmentAsync(int appointmentId);
    Task<bool> RejectAppointmentAsync(int appointmentId);
}