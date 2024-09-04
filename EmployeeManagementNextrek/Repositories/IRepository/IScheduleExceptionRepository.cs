using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IScheduleExceptionRepository:IRepository<ScheduleException>
    {
        Task<ScheduleException> UpDate(ScheduleException exception);
    }
}
