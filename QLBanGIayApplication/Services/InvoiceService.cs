using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay.Models.Models;
using Microsoft.VisualBasic.Devices;

namespace QLBanGiay_Application.Services
{
    public class InvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public InvoiceService(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        public IEnumerable<Invoice> GetAllInvoices()
        {
            return _invoiceRepository.GetAllInvoices();
        }

        public Invoice GetInvoiceById(long invoiceId)
        {
            return _invoiceRepository.GetInvoiceById(invoiceId);
        }

        public void AddInvoice(Invoice invoice)
        {
            _invoiceRepository.AddInvoice(invoice);
        }

        public void UpdateInvoice(Invoice invoice)
        {
            _invoiceRepository.UpdateInvoice(invoice);
        }

        public void DeleteInvoice(long invoiceId)
        {
            _invoiceRepository.DeleteInvoice(invoiceId);
        }

        public IEnumerable<Invoice> GetInvoicesByEmployee(long employeeId)
        {
            return _invoiceRepository.GetInvoicesByEmployee(employeeId);
        }

        public IEnumerable<Invoice> SearchInvoices(string keyword)
        {
            return _invoiceRepository.SearchInvoices(keyword);
        }
    }
}
