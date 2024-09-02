using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class DepartmentCreateDto
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required, MaxLength(100)]
        public required string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Self-referencing Foreign Key for Manager
        public int? ManagerID { get; set; }
        public Employee Manager { get; set; }
    }
}
