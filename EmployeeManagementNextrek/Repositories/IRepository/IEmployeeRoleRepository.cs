using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IEmployeeRoleRepository: IRepository<EmployeeRole>
    {
        Task<EmployeeRole> UpDate(EmployeeRole employee);
        Task<EmployeeRole> GetByIds(int roleId, int employeeId);
    }
}
