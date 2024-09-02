using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IRoleRepository:IRepository<Role>
    {
        Task<Role> UpDate(Role rol);
    }
}
