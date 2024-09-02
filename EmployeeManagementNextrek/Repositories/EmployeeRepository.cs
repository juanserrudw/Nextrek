using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class EmployeeRepository: Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
      

        public async Task<Employee> UpDate(Employee employee)
        {
            employee.UpdatedAt = DateTime.Now;
            _db.Employees.Update(employee);
            await _db.SaveChangesAsync();
            return employee;
        }
    }
}
