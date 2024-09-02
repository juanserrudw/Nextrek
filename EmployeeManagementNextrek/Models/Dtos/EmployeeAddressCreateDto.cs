using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class EmployeeAddressCreateDto
    {
        [Key]
        public int AddressID { get; set; }
        public int EmployeeID { get; set; }
        //public Employee Employee { get; set; }

        [Required, MaxLength(255)]
        public required string StreetAddress { get; set; }
        [Required, MaxLength(25)]
        public required string City { get; set; }
        [Required, MaxLength(100)]
        public required string State { get; set; }
        [Required, MaxLength(12)]
        public required string PostalCode { get; set; }
        [Required, MaxLength(30)]
        public required string Country { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
