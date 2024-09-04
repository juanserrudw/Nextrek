using EmployeeManagementNextrek.Data;
using EmployeeManagementNextrek.Models;
using EmployeeManagementNextrek.Repositories.IRepository;

namespace EmployeeManagementNextrek.Repositories
{
    public class WorkShiftRepository: Repository<WorkShift>, IWorkShiftRepository
    {
        private readonly ApplicationDbContext _db;

        public WorkShiftRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }


        public async Task<WorkShift> UpDate(WorkShift workShift)
        {
            
            _db.WorkShifts.Update(workShift);
            await _db.SaveChangesAsync();
            return workShift;
        }
    }
}
