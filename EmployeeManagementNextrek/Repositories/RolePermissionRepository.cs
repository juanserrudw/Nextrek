using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementNextrek.Repositories
{
    public class RolePermissionRepository:Repository<RolePermission>, IRolePermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public RolePermissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<RolePermission> UpDate(RolePermission rolePermission)
        {
            rolePermission.UpdatedAt = DateTime.Now;
            _db.RolePermissions.Update(rolePermission);
            await _db.SaveChangesAsync();
            return rolePermission;
        }

        public async Task<RolePermission> GetByIds(int roleId, int permisionId)
        {
            return await _db.RolePermissions
                .FirstOrDefaultAsync(er => er.RoleID == roleId && er.PermissionID == permisionId);
        }
    }
}
