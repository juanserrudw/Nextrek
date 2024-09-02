using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using System.Security;

namespace EmployeeManagementNextrek.Repositories
{
    public class PermissionRepository:Repository<Permission>, IPermissionRepository
    {
        private readonly ApplicationDbContext _db;

        public PermissionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Permission> UpDate(Permission permission)
        {
            permission.UpdatedAt = DateTime.Now;
            _db.Permissions.Update(permission);
            await _db.SaveChangesAsync();
            return permission;
        }
    }
}
