using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 3,
                    Name = "Mark",
                    Department = Department.HR,
                    Email = "mark"
                },
                 new Employee
                 {
                     Id = 2,
                     Name = "pop",
                     Department = Department.HR,
                     Email = "pop"
                 }
                );
        }
    }
}
