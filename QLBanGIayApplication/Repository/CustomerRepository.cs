using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly QlShopBanGiayContext _context;

        public CustomerRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer GetCustomerById(long customerId)
        {
            return _context.Customers.FirstOrDefault(c => c.Customerid == customerId);
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void DeleteCustomer(long customerId)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Customerid == customerId);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Customer> SearchCustomers(string keyword)
        {
            return _context.Customers
                .Where(c => c.Customername.ToLower().Contains(keyword) || c.Phonenumber.Contains(keyword))
                .ToList();
        }
    }
}
