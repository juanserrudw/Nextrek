using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class EmployeeAddressRepository:Repository<EmployeeAddress>, IEmployeeAddressRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeAddressRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<EmployeeAddress> UpDate(EmployeeAddress address)
        {
            address.UpdatedAt = DateTime.Now;
            _db.EmployeeAddresses.Update(address);
            await _db.SaveChangesAsync();
            return address;
        }
    }
}
