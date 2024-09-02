using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IEmployeeDocumentRepository:IRepository<EmployeeDocument>
    {
        Task<EmployeeDocument> UpDate(EmployeeDocument employee);
    }
}
