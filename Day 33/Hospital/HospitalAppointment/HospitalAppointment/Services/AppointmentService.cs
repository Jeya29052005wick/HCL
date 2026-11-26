using HospitalAppointment.DTOs;
using HospitalAppointment.Enums;
using HospitalAppointment.Models;
using HospitalAppointment.Repositories.Interfaces;
using HospitalAppointment.Services.Interfaces;

namespace HospitalAppointment.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;

    public AppointmentService(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public async Task<AppointmentResponse> BookAppointmentAsync(
        int patientId, CreateAppointmentRequest request)
    {
        var appointment = new Appointment
        {
            PatientId = patientId,
            Reason = request.Reason,
            AppointmentDate = DateOnly.FromDateTime(request.AppointmentDate),
            AppointmentTime = TimeOnly.FromTimeSpan(request.AppointmentTime),
            Status = AppointmentStatus.Pending
        };

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveAsync();

        return new AppointmentResponse
        {
            Id = appointment.Id,
            PatientId = appointment.PatientId,
            Reason = appointment.Reason,
            AppointmentDate = appointment.AppointmentDate,
            AppointmentTime = appointment.AppointmentTime,
            Status = appointment.Status.ToString()
        };
    }

    public async Task<List<AppointmentResponse>> GetMyAppointmentsAsync(int patientId)
    {
        var appointments = await _appointmentRepository.GetByPatientIdAsync(patientId);

        return appointments.Select(a => new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            PatientName = a.Patient?.Name ?? string.Empty,
            Reason = a.Reason,
            AppointmentDate = a.AppointmentDate,
            AppointmentTime = a.AppointmentTime,
            Status = a.Status.ToString()
        }).ToList();
    }

    public async Task<List<AppointmentResponse>> GetAllAppointmentsAsync()
    {
        var appointments = await _appointmentRepository.GetAllAsync();

        return appointments.Select(a => new AppointmentResponse
        {
            Id = a.Id,
            PatientId = a.PatientId,
            PatientName = a.Patient.Name,
            Reason = a.Reason,
            AppointmentDate = a.AppointmentDate,
            AppointmentTime = a.AppointmentTime,
            Status = a.Status.ToString()
        }).ToList();
    }

    public async Task<bool> ApproveAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

        if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            return false;

        appointment.Status = AppointmentStatus.Approved;

        await _appointmentRepository.SaveAsync();

        return true;
    }

    public async Task<bool> RejectAppointmentAsync(int appointmentId)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(appointmentId);

        if (appointment == null || appointment.Status != AppointmentStatus.Pending)
            return false;

        appointment.Status = AppointmentStatus.Rejected;

        await _appointmentRepository.SaveAsync();

        return true;
    }
}