using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.Models;
using TrainingManagementApi.Services;

namespace TrainingManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly TrainingStore _store;

    public EnrollmentsController(TrainingStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult GetEnrollments()
    {
        return Ok(_store.Enrollments);
    }

    [HttpPost]
    public IActionResult EnrollLearner(
        [FromBody] EnrollmentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.LearnerId) ||
            string.IsNullOrWhiteSpace(request.CourseCode))
        {
            return BadRequest(new
            {
                message = "Learner ID and course code are required."
            });
        }

        var learner = _store.Learners.FirstOrDefault(l =>
            l.LearnerId.Equals(request.LearnerId.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (learner is null)
        {
            return NotFound(new { message = "Learner not found." });
        }

        var course = _store.Courses.FirstOrDefault(c =>
            c.CourseCode.Equals(request.CourseCode.Trim(),
                StringComparison.OrdinalIgnoreCase));

        if (course is null)
        {
            return NotFound(new { message = "Course not found." });
        }

        if (course.AvailableSeats <= 0)
        {
            return Conflict(new { message = "No seats are available." });
        }

        bool alreadyEnrolled = _store.Enrollments.Any(e =>
            e.LearnerId.Equals(learner.LearnerId,
                StringComparison.OrdinalIgnoreCase) &&
            e.CourseCode.Equals(course.CourseCode,
                StringComparison.OrdinalIgnoreCase) &&
            e.Status == "Registered");

        if (alreadyEnrolled)
        {
            return Conflict(new
            {
                message = "This learner is already enrolled in that course."
            });
        }

        var enrollment = new Enrollment
        {
            EnrollmentId = _store.NextEnrollmentId++,
            LearnerId = learner.LearnerId,
            LearnerName = learner.Name,
            CourseCode = course.CourseCode,
            CourseTitle = course.Title,
            EnrollmentDate = DateTime.Now,
            Status = "Registered"
        };

        _store.Enrollments.Add(enrollment);
        course.EnrolledCount++;

        return Ok(enrollment);
    }

    [HttpDelete("{id:int}")]
    public IActionResult CancelEnrollment(int id)
    {
        var enrollment = _store.Enrollments.FirstOrDefault(e =>
            e.EnrollmentId == id);

        if (enrollment is null)
        {
            return NotFound(new
            {
                message = "Enrollment record not found."
            });
        }

        if (enrollment.Status == "Cancelled")
        {
            return Conflict(new
            {
                message = "This enrollment is already cancelled."
            });
        }

        enrollment.Status = "Cancelled";

        var course = _store.Courses.FirstOrDefault(c =>
            c.CourseCode.Equals(enrollment.CourseCode,
                StringComparison.OrdinalIgnoreCase));

        if (course is not null && course.EnrolledCount > 0)
        {
            course.EnrolledCount--;
        }

        return Ok(new
        {
            message = "Enrollment cancelled successfully."
        });
    }
}

public class EnrollmentRequest
{
    public string LearnerId { get; set; } = "";
    public string CourseCode { get; set; } = "";
}