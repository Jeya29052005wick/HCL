
using EmployeeManagement.DTOs.Employee;

namespace EmployeeManagement.Services.Interfaces;

public interface IEmployeeService
{
    Task<List<EmployeeResponseDto>> GetAllAsync();

    Task<EmployeeResponseDto?> GetByIdAsync(int id);

    Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto);

    Task<EmployeeResponseDto?> UpdateAsync(
        int id,
        UpdateEmployeeDto dto
    );

    Task<bool> DeleteAsync(int id);
}
