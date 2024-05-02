using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DAL.Models
{
    // Model
    public class Department : BaseEntity
    {

        [Required(ErrorMessage = "Code is Required!!")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is Required!!")]
        public string Name { get; set; }


        [DisplayName("Date Of Creation")]
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> Employees { get; set; } 

    }
}
