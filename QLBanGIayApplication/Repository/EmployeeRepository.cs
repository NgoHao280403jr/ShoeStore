using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly QlShopBanGiayContext _context;

        public EmployeeRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public Employee GetEmployeeById(long employeeId)
        {
            return _context.Employees.FirstOrDefault(e => e.Employeeid == employeeId);
        }

        public void AddEmployee(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }

        public void DeleteEmployee(long employeeId)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.Employeeid == employeeId);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Employee> SearchEmployees(string keyword)
        {
            return _context.Employees
                .Where(e => e.Employeename.ToLower().Contains(keyword) ||
                            e.Phonenumber.Contains(keyword))
                .ToList();
        }
    }
}
