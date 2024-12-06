using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;

namespace QLBanGiay_Application.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return _customerRepository.GetAllCustomers();
        }

        public Customer GetCustomerById(long customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }

        public void AddCustomer(Customer customer)
        {
            _customerRepository.AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepository.UpdateCustomer(customer);
        }

        public void DeleteCustomer(long customerId)
        {
            _customerRepository.DeleteCustomer(customerId);
        }

        public IEnumerable<Customer> SearchCustomers(string keyword)
        {
            return _customerRepository.SearchCustomers(keyword);
        }
    }
}
