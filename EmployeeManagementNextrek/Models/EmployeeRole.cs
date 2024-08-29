namespace EmployeeManagementNextrek.Models
{
    public class EmployeeRole
    {
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public DateTime AssignedAt { get; set; }
        public DateTime? RevokedAt { get; set; }
    }
}
