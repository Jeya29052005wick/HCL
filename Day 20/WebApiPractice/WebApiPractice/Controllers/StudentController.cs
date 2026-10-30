using Microsoft.AspNetCore.Mvc;
using WebApiPractice.DTOs;
using WebApiPractice.Services;

namespace WebApiPractice.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    // GET: api/student
    [HttpGet]
    public async Task<IActionResult> GetAllStudents()
    {
        var students = await _studentService.GetAllStudentsAsync();

        return Ok(students);
    }

    // GET: api/student/5
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid student ID.");

        var student = await _studentService.GetStudentByIdAsync(id);

        if (student == null)
            return NotFound("Student not found.");

        return Ok(student);
    }

    // POST: api/student
    [HttpPost]
    public async Task<IActionResult> CreateStudent(
        [FromBody] CreateStudentDto studentDto)
    {
        if (await _studentService.EmailExistsAsync(studentDto.Email))
            return Conflict("A student with this email already exists.");

        var student = await _studentService.AddStudentAsync(studentDto);

        return CreatedAtAction(
            nameof(GetStudentById),
            new { id = student.Id },
            student);
    }

    // PUT: api/student/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateStudent(
        int id,
        [FromBody] UpdateStudentDto studentDto)
    {
        if (id <= 0)
            return BadRequest("Invalid student ID.");

        if (await _studentService.EmailExistsAsync(
            studentDto.Email, id))
        {
            return Conflict("A student with this email already exists.");
        }

        var updatedStudent =
            await _studentService.UpdateStudentAsync(id, studentDto);

        if (updatedStudent == null)
            return NotFound("Student not found.");

        return Ok(updatedStudent);
    }

    // DELETE: api/student/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (id <= 0)
            return BadRequest("Invalid student ID.");

        var deleted = await _studentService.DeleteStudentAsync(id);

        if (!deleted)
            return NotFound("Student not found.");

        return NoContent();
    }
}