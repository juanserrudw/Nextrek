using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class RoleRepository:Repository<Role>, IRoleRepository
    {
        private readonly ApplicationDbContext _db;

        public RoleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Role> UpDate(Role rol)
        {
            rol.UpdatedAt = DateTime.Now;
            _db.Roles.Update(rol);
            await _db.SaveChangesAsync();
            return rol;
        }
    }
}
