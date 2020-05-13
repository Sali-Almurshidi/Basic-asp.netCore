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
            new Employee(){Id=1, Name="MAry",Department=Department.HR,Email="mary@gmail.com"},
            new Employee(){Id=2, Name="sam",Department=Department.Payroll,Email="sam@gmail.com"},
            new Employee(){Id=3, Name="rrr",Department=Department.None,Email="rrr@gmail.com"}
            };
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1;
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == id);
            if (employee != null)
            {
                _employeeList.Remove(employee);
            }
            return employee;
        }

        public Employee GetEmployee(int Id)
        {
            //throw new NotImplementedException();
            return _employeeList.FirstOrDefault(e => e.Id == Id);
        }

        public Employee Update(Employee employeeChanges)
        {
            Employee employee = _employeeList.FirstOrDefault(e => e.Id == employeeChanges.Id);
            if (employee != null)
            {
                employee.Name = employeeChanges.Name;
                employee.Email = employeeChanges.Email;
                employee.Department = employeeChanges.Department;
            }
            return employee;
        }

        //Employee IEmployeeRepository.GetEmployee(int Id)
        //{
        //    throw new NotImplementedException();
        //    //return _employeeList;
        //}

        IEnumerable<Employee> IEmployeeRepository.GetEmployees()
        {
            //throw new NotImplementedException();
            return _employeeList;
        }
    }
}
