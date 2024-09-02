using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IPermissionRepository:IRepository<Permission>
    {
        Task<Permission> UpDate(Permission employee);
    }
}
