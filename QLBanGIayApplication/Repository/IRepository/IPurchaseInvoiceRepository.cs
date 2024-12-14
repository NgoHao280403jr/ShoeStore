using QLBanGiay.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IPurchaseInvoiceRepository
    {
        IEnumerable<PurchaseInvoice> GetAllInvoices();
        PurchaseInvoice GetInvoiceById(long invoiceId);
        void AddInvoice(PurchaseInvoice purchaseInvoice);
        void UpdateInvoice(PurchaseInvoice purchaseInvoice);
        void DeleteInvoice(long invoiceId);
        IEnumerable<PurchaseInvoice> SearchInvoices(string keyword);
    }
}
