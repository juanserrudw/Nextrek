using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class ScheduleExceptionRepository: Repository<ScheduleException>, IScheduleExceptionRepository
    {
        private readonly ApplicationDbContext _db;

        public ScheduleExceptionRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<ScheduleException> UpDate(ScheduleException exception)
        {
            
            _db.ScheduleExceptions.Update(exception);
            await _db.SaveChangesAsync();
            return exception;
        }
    }
}
