using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>() {
            new Employee(){Id=1, Name="MAry",Department="HR",Email="mary@gmail.com"},
            new Employee(){Id=2, Name="sam",Department="HR",Email="sam@gmail.com"},
            new Employee(){Id=3, Name="rrr",Department="HR",Email="rrr@gmail.com"}
            };
        }
        public Employee GetEmployee(int Id)
        {
            //throw new NotImplementedException();
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        Employee IEmployeeRepository.GetEmployee(int Id)
        {
            throw new NotImplementedException();
            //return _employeeList;
        }

        IEnumerable<Employee> IEmployeeRepository.GetEmployees()
        {
            //throw new NotImplementedException();
            return _employeeList;
        }
    }
}
