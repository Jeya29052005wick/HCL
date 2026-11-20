
using EmployeeManagement.DTOs.Department;
using EmployeeManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers;

[Authorize(Roles = "Manager")]
[ApiController]
[Route("api/[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    // GET: api/department
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var departments = await _departmentService.GetAllAsync();

        return Ok(departments);
    }

    // GET: api/department/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department == null)
            return NotFound(new { message = "Department not found." });

        return Ok(department);
    }

    // POST: api/department
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = department.Id },
                department
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/department/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateDepartmentDto dto)
    {
        try
        {
            var department = await _departmentService.UpdateAsync(id, dto);

            if (department == null)
                return NotFound(new { message = "Department not found." });

            return Ok(department);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/department/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool deleted = await _departmentService.DeleteAsync(id);

        if (!deleted)
            return NotFound(new { message = "Department not found." });

        return Ok(new { message = "Department deleted successfully." });
    }
}
