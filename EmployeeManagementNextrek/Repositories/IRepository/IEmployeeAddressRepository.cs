using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IEmployeeAddressRepository: IRepository<EmployeeAddress>
    {
        Task<EmployeeAddress> UpDate(EmployeeAddress employee);
    }
}
