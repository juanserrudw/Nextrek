using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagementNextrek.Models
{
    public class EmployeeDocument
    {
        [Key]
        public int DocumentID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        public Employee Employee { get; set; }

        [Required, MaxLength(50)]
        public string? DocumentType { get; set; }
        [Required, MaxLength(255)]
        public string? DocumentPath { get; set; }

        public DateTime UploadedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
