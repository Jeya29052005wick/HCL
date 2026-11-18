
namespace EmployeeManagement.Models;

public class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string? Address { get; set; }

    public int DepartmentId { get; set; }

    public DateTime JoiningDate { get; set; }

    // Navigation property
    public Department Department { get; set; } = null!;
}
