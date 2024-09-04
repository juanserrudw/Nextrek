using EmployeeManagementNextrek.Models;

namespace EmployeeManagementNextrek.Repositories.IRepository
{
    public interface IWorkShiftRepository: IRepository<WorkShift>
    {
        Task<WorkShift> UpDate(WorkShift workShift);
    }
}
