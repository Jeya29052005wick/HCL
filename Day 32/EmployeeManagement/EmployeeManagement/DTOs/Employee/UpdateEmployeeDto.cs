
namespace EmployeeManagement.DTOs.Employee;

public class UpdateEmployeeDto
{
    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Address { get; set; }

    public int DepartmentId { get; set; }

    public DateTime JoiningDate { get; set; }
}
