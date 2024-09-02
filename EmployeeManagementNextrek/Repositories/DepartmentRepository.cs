using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class DepartmentRepository : Repository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _db;

        public DepartmentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<Department> UpDate(Department department)
        {
            department.UpdatedAt = DateTime.Now;
            _db.Departments.Update(department);
            await _db.SaveChangesAsync();
            return department;
        }
    }
}
