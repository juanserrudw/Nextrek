using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IEmployeeRepository: IRepository<Employee>
    {
        Task<Employee> UpDate(Employee employee); 
    }
}
