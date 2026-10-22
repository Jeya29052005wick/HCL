using TrainingManagementApi.Models;

namespace TrainingManagementApi.Services;

public class TrainingStore
{
    public List<Course> Courses { get; } = new()
    {
        new Course
        {
            CourseCode = "CS101",
            Title = "C# Programming",
            Fee = 5000m,
            DurationDays = 30,
            TotalSeats = 40
        },
        new Course
        {
            CourseCode = "CS102",
            Title = "Java Programming",
            Fee = 6000m,
            DurationDays = 35,
            TotalSeats = 35
        },
        new Course
        {
            CourseCode = "WD101",
            Title = "Web Development",
            Fee = 4500m,
            DurationDays = 25,
            TotalSeats = 30
        },
        new Course
        {
            CourseCode = "PY101",
            Title = "Python Programming",
            Fee = 5500m,
            DurationDays = 30,
            TotalSeats = 25
        },
        new Course
        {
            CourseCode = "DS101",
            Title = "Data Science",
            Fee = 8000m,
            DurationDays = 45,
            TotalSeats = 20
        }
    };

    public List<Learner> Learners { get; } = new();

    public List<Enrollment> Enrollments { get; } = new();

    public int NextEnrollmentId { get; set; } = 1;
}