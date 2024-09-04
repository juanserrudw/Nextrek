using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class WorkScheduleDto
    {
        [Key]
        public int ScheduleId { get; set; }
        public int EmployeeId { get; set; }

        public string DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsFlexible { get; set; }
        public string Notes { get; set; }
    }
}
