namespace HospitalAppointment.DTOs;

public class CreateAppointmentRequest
{
    public string Reason { get; set; } = string.Empty;
    public DateTime AppointmentDate { get; set; }
    public TimeSpan AppointmentTime { get; set; }
}