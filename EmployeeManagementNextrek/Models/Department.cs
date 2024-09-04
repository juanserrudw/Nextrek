using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementNextrek.Models
{
    public class Department
    {
        [Key]
        public int DepartmentID { get; set; }
        [Required, MaxLength(100)]
        public required string DepartmentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Self-referencing Foreign Key for Manager
       // public int? ManagerID { get; set; }

        //[ForeignKey(nameof(ManagerID))]
       // public Employee? Manager { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}
