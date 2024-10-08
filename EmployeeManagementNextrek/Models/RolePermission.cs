﻿namespace EmployeeManagementNextrek.Models
{
    public class RolePermission
    {
        public int RolePermissionId { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public int PermissionID { get; set; }
        public Permission Permission { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
