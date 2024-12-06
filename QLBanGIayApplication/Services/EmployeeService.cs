using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;

namespace QLBanGiay_Application.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeRepository.GetAllEmployees();
        }

        public Employee GetEmployeeById(long employeeId)
        {
            return _employeeRepository.GetEmployeeById(employeeId);
        }

        public void AddEmployee(Employee employee)
        {
            _employeeRepository.AddEmployee(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.UpdateEmployee(employee);
        }

        public void DeleteEmployee(long employeeId)
        {
            _employeeRepository.DeleteEmployee(employeeId);
        }

        public IEnumerable<Employee> SearchEmployees(string keyword)
        {
            return _employeeRepository.SearchEmployees(keyword);
        }
    }
}
