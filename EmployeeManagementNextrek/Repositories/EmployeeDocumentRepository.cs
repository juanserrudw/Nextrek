using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class EmployeeDocumentRepository:Repository<EmployeeDocument>,IEmployeeDocumentRepository 
    {
        private readonly ApplicationDbContext _db;

        public EmployeeDocumentRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<EmployeeDocument> UpDate(EmployeeDocument employeeDocument)
        {
            employeeDocument.UpdatedAt = DateTime.Now;
            _db.EmployeeDocuments.Update(employeeDocument);
            await _db.SaveChangesAsync();
            return employeeDocument;
        }
    }
}
