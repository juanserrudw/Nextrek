using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class ScheduleExceptionDto
    {
        [Key]
        public int ExceptionId { get; set; }
        public int ScheduleId { get; set; }
        public DateTime ExceptionDate { get; set; }
        public TimeSpan? StartTimeOverride { get; set; }
        public TimeSpan? EndTimeOverride { get; set; }
        public string Reason { get; set; }
    }
}
