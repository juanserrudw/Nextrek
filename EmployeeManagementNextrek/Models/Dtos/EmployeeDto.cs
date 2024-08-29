﻿using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementNextrek.Models.Dtos
{
    public class EmployeeDto
    {
       
        public int EmployeeID { get; set; }
      
        public required string FirstName { get; set; }
       
        public required string LastName { get; set; }
        
        public required string Email { get; set; }
       
        public required string PhoneNumber { get; set; }
    }
}
