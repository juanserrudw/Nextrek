using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        [Required, MaxLength(100)]
        public required string RoleName { get; set; }
        [MaxLength(500)]
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<EmployeeRole> EmployeeRoles { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
