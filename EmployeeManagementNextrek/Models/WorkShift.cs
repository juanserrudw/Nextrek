using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class WorkShift
    {
        [Key]
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsActive { get; set; }

        // Relación con WorkSchedules (opcional)
        public ICollection<WorkSchedule> WorkSchedules { get; set; }
    }
}
