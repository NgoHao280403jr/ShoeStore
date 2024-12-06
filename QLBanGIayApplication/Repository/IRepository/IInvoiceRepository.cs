using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IInvoiceRepository
    {
        IEnumerable<Invoice> GetAllInvoices();
        Invoice GetInvoiceById(long invoiceId);
        void AddInvoice(Invoice invoice);
        void UpdateInvoice(Invoice invoice);
        void DeleteInvoice(long invoiceId);
        IEnumerable<Invoice> GetInvoicesByEmployee(long employeeId);
        IEnumerable<Invoice> SearchInvoices(string phoneNumber, string paymentMethod);
    }
}
