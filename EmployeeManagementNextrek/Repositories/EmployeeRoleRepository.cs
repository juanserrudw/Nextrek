using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementNextrek.Repositories
{
    public class EmployeeRoleRepository:Repository<EmployeeRole>, IEmployeeRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<EmployeeRole> UpDate(EmployeeRole role)
        {
            _db.EmployeeRoles.Update(role);
            await _db.SaveChangesAsync();
            return role;
        }

        public async Task<EmployeeRole> GetByIds(int roleId, int employeeId)
        {
            return await _db.EmployeeRoles
                .FirstOrDefaultAsync(er => er.RoleID == roleId && er.EmployeeID == employeeId);
        }
    }
}
