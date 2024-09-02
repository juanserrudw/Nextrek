using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class PermissionUpDateDto
    {
        [Key]
        public int PermissionID { get; set; }
        [Required, MaxLength(100)]
        public required string PermissionName { get; set; }
        [MaxLength(255)]
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
