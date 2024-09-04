using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class EmployeeRole
    {
        [Key]
        public int EmployeRoleId { get; set; }
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
