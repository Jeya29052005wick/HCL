
using EmployeeManagement.Models;

namespace EmployeeManagement.Repositories.Interfaces;

public interface IDepartmentRepository
{
    Task<List<Department>> GetAllAsync();

    Task<Department?> GetByIdAsync(int id);

    Task<bool> ExistsByNameAsync(string name);

    Task AddAsync(Department department);

    Task UpdateAsync(Department department);

    Task DeleteAsync(Department department);
}
