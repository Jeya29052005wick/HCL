using Microsoft.AspNetCore.Mvc;
using TrainingManagementApi.Models;
using TrainingManagementApi.Services;

namespace TrainingManagementApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly TrainingStore _store;

    public CoursesController(TrainingStore store)
    {
        _store = store;
    }

    [HttpGet]
    public IActionResult GetCourses(
        [FromQuery] string? search,
        [FromQuery] string? sortBy)
    {
        IEnumerable<Course> courses = _store.Courses;

        if (!string.IsNullOrWhiteSpace(search))
        {
            courses = courses.Where(c =>
                c.CourseCode.Contains(search,
                    StringComparison.OrdinalIgnoreCase) ||
                c.Title.Contains(search,
                    StringComparison.OrdinalIgnoreCase));
        }

        if (string.Equals(sortBy, "fee",
            StringComparison.OrdinalIgnoreCase))
        {
            courses = courses.OrderBy(c => c.Fee);
        }
        else if (string.Equals(sortBy, "fee-desc",
            StringComparison.OrdinalIgnoreCase))
        {
            courses = courses.OrderByDescending(c => c.Fee);
        }

        return Ok(courses);
    }

    [HttpGet("{courseCode}")]
    public IActionResult GetCourse(string courseCode)
    {
        var course = _store.Courses.FirstOrDefault(c =>
            c.CourseCode.Equals(courseCode,
                StringComparison.OrdinalIgnoreCase));

        return course is null
            ? NotFound(new { message = "Course not found." })
            : Ok(course);
    }

    [HttpPost]
    public IActionResult AddCourse(Course course)
    {
        if (string.IsNullOrWhiteSpace(course.CourseCode) ||
            string.IsNullOrWhiteSpace(course.Title))
        {
            return BadRequest(new
            {
                message = "Course code and title are required."
            });
        }

        if (course.Fee < 0 ||
            course.DurationDays <= 0 ||
            course.TotalSeats <= 0)
        {
            return BadRequest(new
            {
                message = "Fee must be non-negative; duration and seats must be positive."
            });
        }

        if (_store.Courses.Any(c =>
            c.CourseCode.Equals(course.CourseCode.Trim(),
                StringComparison.OrdinalIgnoreCase)))
        {
            return Conflict(new
            {
                message = "That course code already exists."
            });
        }

        course.CourseCode = course.CourseCode.Trim();
        course.Title = course.Title.Trim();
        course.EnrolledCount = 0;

        _store.Courses.Add(course);

        return CreatedAtAction(
            nameof(GetCourse),
            new { courseCode = course.CourseCode },
            course);
    }
}