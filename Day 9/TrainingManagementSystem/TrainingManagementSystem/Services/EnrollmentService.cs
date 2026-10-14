
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Services;

public class EnrollmentService
{
    private readonly Course[] courses;
    private readonly LearnerService learnerService;
    private readonly List<Enrollment> enrollments = new();

    private int nextEnrollmentId = 1;

    public EnrollmentService(
        Course[] courses,
        LearnerService learnerService)
    {
        this.courses = courses;
        this.learnerService = learnerService;
    }

    private Course? FindCourse(string courseCode)
    {
        foreach (Course course in courses)
        {
            if (course.CourseCode.Equals(
                courseCode, StringComparison.OrdinalIgnoreCase))
            {
                return course;
            }
        }

        return null;
    }

    public void EnrollLearner()
    {
        Console.Write("Enter learner ID: ");
        string learnerId = Console.ReadLine()?.Trim() ?? "";

        if (!learnerService.TryGetLearner(
            learnerId, out Learner learner))
        {
            Console.WriteLine("Learner not found. Register first.");
            return;
        }

        Console.Write("Enter course code: ");
        string courseCode = Console.ReadLine()?.Trim() ?? "";

        Course? course = FindCourse(courseCode);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        foreach (Enrollment enrollment in enrollments)
        {
            if (enrollment.LearnerId == learner.LearnerId &&
                enrollment.CourseCode.Equals(
                    course.CourseCode,
                    StringComparison.OrdinalIgnoreCase) &&
                enrollment.Status == EnrollmentStatus.Active)
            {
                Console.WriteLine("Learner is already actively enrolled.");
                return;
            }
        }

        if (course.AvailableSeats <= 0)
        {
            Console.WriteLine("No seats available for this course.");
            return;
        }

        Enrollment newEnrollment = new Enrollment(
            nextEnrollmentId++,
            learner.LearnerId,
            course.CourseCode,
            course.Fee,
            0m,
            course.Fee);

        enrollments.Add(newEnrollment);
        course.EnrolledCount++;

        Console.WriteLine("\nEnrollment successful!");
        Console.WriteLine($"Enrollment ID: {newEnrollment.EnrollmentId}");
        Console.WriteLine($"Learner: {learner.Name}");
        Console.WriteLine($"Course: {course.Title}");
        Console.WriteLine($"Base fee: Rs. {newEnrollment.BaseFee:F2}");
        Console.WriteLine($"Final fee: Rs. {newEnrollment.FinalFee:F2}");
        Console.WriteLine($"Available seats: {course.AvailableSeats}");
    }

    public void DisplayEnrollments()
    {
        if (enrollments.Count == 0)
        {
            Console.WriteLine("No enrollment records found.");
            return;
        }

        Console.WriteLine("\n--- ENROLLMENT RECORDS ---");

        foreach (Enrollment enrollment in enrollments)
        {
            Console.WriteLine(
                $"ID: {enrollment.EnrollmentId} | " +
                $"Learner: {enrollment.LearnerId} | " +
                $"Course: {enrollment.CourseCode} | " +
                $"Fee: Rs. {enrollment.FinalFee:F2} | " +
                $"Status: {enrollment.Status}");
        }
    }

    public void CancelEnrollment()
    {
        Console.Write("Enter enrollment ID to cancel: ");
        string input = Console.ReadLine() ?? "";

        if (!int.TryParse(input, out int enrollmentId))
        {
            Console.WriteLine("Enter a valid enrollment ID.");
            return;
        }

        Enrollment? selectedEnrollment = null;

        foreach (Enrollment enrollment in enrollments)
        {
            if (enrollment.EnrollmentId == enrollmentId)
            {
                selectedEnrollment = enrollment;
                break;
            }
        }

        if (selectedEnrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        if (selectedEnrollment.Status == EnrollmentStatus.Cancelled)
        {
            Console.WriteLine("This enrollment is already cancelled.");
            return;
        }

        selectedEnrollment.Status = EnrollmentStatus.Cancelled;

        Course? course = FindCourse(selectedEnrollment.CourseCode);

        if (course != null)
        {
            course.EnrolledCount--;
        }

        Console.WriteLine("Enrollment cancelled successfully.");
        Console.WriteLine("One seat has been released.");
    }
}
