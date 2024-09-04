using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class WorkSchedule
    {
        [Key]
        public int ScheduleId { get; set; }
        public int EmployeeId { get; set; }

        public string DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsFlexible { get; set; }
        public string Notes { get; set; }

        // Relación con Employee
        public Employee Employee { get; set; }

        // Relación con ScheduleExceptions
       
        public ICollection<ScheduleException> ScheduleExceptions { get; set; }
        public int WorkShiftId { get; set; }
        public WorkShift WorkShift { get; set; }
    }
}
