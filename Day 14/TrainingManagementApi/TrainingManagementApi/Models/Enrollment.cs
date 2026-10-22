namespace TrainingManagementApi.Models;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public string LearnerId { get; set; } = "";
    public string LearnerName { get; set; } = "";
    public string CourseCode { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Registered";
}