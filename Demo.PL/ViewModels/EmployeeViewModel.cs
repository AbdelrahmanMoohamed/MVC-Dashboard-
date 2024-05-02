using Demo.DAL.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using Microsoft.AspNetCore.Http;

namespace Demo.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }

        [Range(22, 45)]
        public int? Age { get; set; }

        public decimal Salary { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address Must be Like 123-Street-City-Country")]
        public string Address { get; set; } // 123-Street-City-Country

        [Phone]
        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HireDate { get; set; }

        [DisplayName("Date Of Create")]
        public DateTime DateOfCreate { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }

        public int? DepartmentId { get; set; } // FK
        public Department Department { get; set; } // Navigational Propety
    }
}
