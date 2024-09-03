using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IRolePermissionRepository: IRepository<RolePermission>
    {
        Task<RolePermission> UpDate(RolePermission rolePermission);
        Task<RolePermission> GetByIds(int roleId, int employeeId);
    }
}
