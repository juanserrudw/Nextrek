using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        
            Task<Department> UpDate(Department edepartment);
        
    }
}
