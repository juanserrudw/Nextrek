using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class EmployeeDocumentDto
    {
        [Key]
        public int DocumentID { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeID { get; set; }
        //public Employee Employee { get; set; }

        [Required, MaxLength(50)]
        public string? DocumentType { get; set; }
        [Required, MaxLength(255)]
        public string? DocumentPath { get; set; }

        public DateTime UploadedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
