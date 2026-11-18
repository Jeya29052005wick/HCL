
using EmployeeManagement.DTOs.Employee;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using EmployeeManagement.Services.Interfaces;

namespace EmployeeManagement.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<EmployeeResponseDto>> GetAllAsync()
    {
        var employees = await _repository.GetAllAsync();

        return employees.Select(e => MapToResponse(e)).ToList();
    }

    public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return null;

        return MapToResponse(employee);
    }

    public async Task<EmployeeResponseDto> CreateAsync(
        CreateEmployeeDto dto)
    {
        string email = dto.Email.Trim();

        if (await _repository.ExistsByEmailAsync(email))
            throw new InvalidOperationException(
                "Employee email already exists."
            );

        if (!await _repository.DepartmentExistsAsync(dto.DepartmentId))
            throw new InvalidOperationException(
                "Selected department does not exist."
            );

        var employee = new Employee
        {
            Name = dto.Name.Trim(),
            Email = email,
            Phone = dto.Phone.Trim(),
            Address = dto.Address?.Trim(),
            DepartmentId = dto.DepartmentId,
            JoiningDate = dto.JoiningDate
        };

        await _repository.AddAsync(employee);

        var savedEmployee = await _repository.GetByIdAsync(employee.Id);

        return MapToResponse(savedEmployee!);
    }

    public async Task<EmployeeResponseDto?> UpdateAsync(
        int id,
        UpdateEmployeeDto dto)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return null;

        string email = dto.Email.Trim();

        if (!string.Equals(
            employee.Email,
            email,
            StringComparison.OrdinalIgnoreCase
        ))
        {
            if (await _repository.ExistsByEmailAsync(email))
                throw new InvalidOperationException(
                    "Employee email already exists."
                );
        }

        if (!await _repository.DepartmentExistsAsync(dto.DepartmentId))
            throw new InvalidOperationException(
                "Selected department does not exist."
            );

        employee.Name = dto.Name.Trim();
        employee.Email = email;
        employee.Phone = dto.Phone.Trim();
        employee.Address = dto.Address?.Trim();
        employee.DepartmentId = dto.DepartmentId;
        employee.JoiningDate = dto.JoiningDate;

        await _repository.UpdateAsync(employee);

        return MapToResponse(employee);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var employee = await _repository.GetByIdAsync(id);

        if (employee == null)
            return false;

        await _repository.DeleteAsync(employee);

        return true;
    }

    private static EmployeeResponseDto MapToResponse(Employee employee)
    {
        return new EmployeeResponseDto
        {
            Id = employee.Id,
            Name = employee.Name,
            Email = employee.Email,
            Phone = employee.Phone,
            Address = employee.Address,
            DepartmentId = employee.DepartmentId,
            DepartmentName = employee.Department.Name,
            JoiningDate = employee.JoiningDate
        };
    }
}
