
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagement.Api.Data;
using StudentManagement.Api.Models;

namespace StudentManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBaseA
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/students
        // Get all students
        [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            var students = await _context.Students
                .AsNoTracking()
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/students/5
        // Get a student by ID
        [ProducesResponseType(typeof(Student), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            return Ok(student);
        }

        // POST: api/students
        // Create a new student
        [HttpPost]
        [ProducesResponseType(typeof(Student), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<Student>> CreateStudent(Student student)
        {
            // Check whether the email already exists
            var emailExists = await _context.Students
                .AnyAsync(s => s.Email == student.Email);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "Email already exists."
                });
            }

            _context.Students.Add(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "Unable to create student. The email may already exist."
                });
            }

            return CreatedAtAction(
                nameof(GetStudentById),
                new { id = student.Id },
                student);
        }

        // PUT: api/students/5
        // Update an existing student
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateStudent(
            int id, Student updatedStudent)
        {
            if (id != updatedStudent.Id)
            {
                return BadRequest(new
                {
                    message = "Student ID in URL does not match the ID in the request body."
                });
            }

            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            var emailExists = await _context.Students
                .AnyAsync(s => s.Email == updatedStudent.Email
                            && s.Id != id);

            if (emailExists)
            {
                return Conflict(new
                {
                    message = "Email already exists."
                });
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.Department = updatedStudent.Department;
            student.Age = updatedStudent.Age;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return Conflict(new
                {
                    message = "The update conflicts with existing database data."
                });
            }

            return NoContent();
        }

        // DELETE: api/students/5
        // Delete a student by ID
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                return NotFound(new
                {
                    message = "Student not found."
                });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/students/search?name=Arun&department=IT
        [ProducesResponseType(typeof(IEnumerable<Student>), StatusCodes.Status200OK)]
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Student>>> SearchStudents(
            [FromQuery] string? name,
            [FromQuery] string? department)
        {
            var query = _context.Students.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(s => s.Name.Contains(name));
            }

            if (!string.IsNullOrWhiteSpace(department))
            {
                query = query.Where(s => s.Department.Contains(department));
            }

            var students = await query.ToListAsync();

            return Ok(students);
        }


    }
}
