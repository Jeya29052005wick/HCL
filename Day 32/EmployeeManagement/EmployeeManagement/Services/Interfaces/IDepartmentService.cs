using EmployeeManagement.DTOs.Department;

namespace EmployeeManagement.Services.Interfaces;

public interface IDepartmentService
{
    Task<List<DepartmentResponseDto>> GetAllAsync();

    Task<DepartmentResponseDto?> GetByIdAsync(int id);

    Task<DepartmentResponseDto> CreateAsync(CreateDepartmentDto dto);

    Task<DepartmentResponseDto?> UpdateAsync(
        int id,
        UpdateDepartmentDto dto);

    Task<bool> DeleteAsync(int id);
}