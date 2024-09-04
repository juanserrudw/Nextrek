using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class WorkScheduleRepository:Repository<WorkSchedule>, IWorkScheduleRepository
    {
        private readonly ApplicationDbContext _db;

        public WorkScheduleRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<WorkSchedule> UpDate(WorkSchedule schedule)
        {
            
            _db.WorkSchedules.Update(schedule);
            await _db.SaveChangesAsync();
            return schedule;
        }
    }
}
