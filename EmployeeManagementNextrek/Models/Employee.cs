using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeID { get; set; }
        [MaxLength(30)]
        public required string FirstName { get; set; }
        [MaxLength(100)]
        public required string LastName { get; set; }
        [Required(ErrorMessage = "El campo de correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [MaxLength(256, ErrorMessage = "El correo electrónico no puede tener más de 256 caracteres.")]
        public required string Email { get; set; }
        [MaxLength(15)]
        [Required(ErrorMessage = "El campo de número de teléfono es obligatorio.")]
        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public required string PhoneNumber { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime DateOfBirth { get; set; }

        // Foreign Key for Department
        public int DepartmentID { get; set; }
        public Department Department { get; set; }

        // Self-referencing Foreign Key for Manager
        //public int? ManagerID { get; set; }
        //public Employee Manager { get; set; }
        //public ICollection<Employee> Subordinates { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //public ICollection<EmployeeRole> EmployeeRoles { get; set; }
        //public ICollection<EmployeeAddress> Addresses { get; set; }
        //public ICollection<EmployeeDocument> Documents { get; set; }

        //public ICollection<WorkSchedule> WorkSchedules { get; set; }
    }
}
