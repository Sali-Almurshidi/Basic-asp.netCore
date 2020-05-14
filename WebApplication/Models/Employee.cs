using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "toooo longe")]
        public string Name { get; set; }
        [Required]
        // [RegularExpression(@"@", ErrorMessage = "Invalid Email Format")]
        [Display(Name = "office Email")]
        public string Email { get; set; }
        [Required]
        public Department? Department { get; set; } // enum type because it is list

        public string PhotoPath { get; set; }
    }

}
