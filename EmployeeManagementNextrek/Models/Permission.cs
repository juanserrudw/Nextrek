using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class Permission
    {
        [Key]
        public int PermissionID { get; set; }
        [Required, MaxLength(100)]
        public required string PermissionName { get; set; }
        [MaxLength(255)]
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
