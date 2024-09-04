using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IWorkScheduleRepository: IRepository<WorkSchedule>
    {
        Task<WorkSchedule> UpDate(WorkSchedule schedule);
    }
}
