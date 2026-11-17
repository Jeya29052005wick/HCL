using EmployeeManagement.DTOs.Department;
using EmployeeManagement.Models;
using EmployeeManagement.Repositories.Interfaces;
using EmployeeManagement.Services.Interfaces;

namespace EmployeeManagement.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _repository;

    public DepartmentService(IDepartmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DepartmentResponseDto>> GetAllAsync()
    {
        var departments = await _repository.GetAllAsync();

        return departments.Select(d => new DepartmentResponseDto
        {
            Id = d.Id,
            Name = d.Name,
            Description = d.Description
        }).ToList();
    }

    public async Task<DepartmentResponseDto?> GetByIdAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return null;

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };
    }

    public async Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentDto dto)
    {
        bool exists = await _repository.ExistsByNameAsync(dto.Name);

        if (exists)
            throw new InvalidOperationException(
                "Department name already exists.");

        var department = new Department
        {
            Name = dto.Name.Trim(),
            Description = dto.Description?.Trim()
        };

        await _repository.AddAsync(department);

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };
    }

    public async Task<DepartmentResponseDto?> UpdateAsync(
        int id,
        UpdateDepartmentDto dto)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return null;

        bool exists = await _repository.ExistsByNameAsync(dto.Name);

        if (exists && !string.Equals(
            department.Name,
            dto.Name.Trim(),
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Department name already exists.");
        }

        department.Name = dto.Name.Trim();
        department.Description = dto.Description?.Trim();

        await _repository.UpdateAsync(department);

        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var department = await _repository.GetByIdAsync(id);

        if (department == null)
            return false;

        await _repository.DeleteAsync(department);

        return true;
    }
}