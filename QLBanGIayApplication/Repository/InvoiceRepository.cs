using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay_Application.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly QlShopBanGiayContext _context;

        public InvoiceRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _context.Invoices.Include(i => i.Employee).ToList();
        }
        public Invoice GetInvoiceById(long invoiceId)
        {
            return _context.Invoices.Include(i => i.Employee)
                           .FirstOrDefault(i => i.Invoiceid == invoiceId);
        }
        public void AddInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            _context.SaveChanges();
        }
        public void UpdateInvoice(Invoice invoice)
        {
            _context.Invoices.Update(invoice);
            _context.SaveChanges();
        }
        public void DeleteInvoice(long invoiceId)
        {
            var invoice = GetInvoiceById(invoiceId);
            if (invoice != null)
            {
                _context.Invoices.Remove(invoice);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Invoice> GetInvoicesByEmployee(long employeeId)
        {
            return _context.Invoices
                           .Where(i => i.Employeeid == employeeId)
                           .Include(i => i.Employee)
                           .ToList();
        }
        public IEnumerable<Invoice> SearchInvoices(string keyword)
        {
            return _context.Invoices
                           .Where(i => (string.IsNullOrEmpty(keyword) || i.Phonenumber.Contains(keyword)))
                           .Include(i => i.Employee)
                           .ToList();
        }
    }
}
