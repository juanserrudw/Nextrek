using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class EmployeCreateDto
    {
        public int DepartmentID { get; set; }
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
        public string Status { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
