using HospitalAppointment.Enums;

namespace HospitalAppointment.Models;

public class Appointment
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public User Patient { get; set; } = null!;

    public string Reason { get; set; } = string.Empty;

    public DateOnly AppointmentDate { get; set; }

    public TimeOnly AppointmentTime { get; set; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
}