using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Services
{
    public class PurchaseInvoiceService
    {
        private readonly IPurchaseInvoiceRepository _purchaseInvoiceRepository;
        public PurchaseInvoiceService(IPurchaseInvoiceRepository purchaseInvoiceRepository)
        {
            _purchaseInvoiceRepository = purchaseInvoiceRepository;
        }
        public IEnumerable<PurchaseInvoice> GetAllInvoices()
        {
            return _purchaseInvoiceRepository.GetAllInvoices();
        }

        public PurchaseInvoice GetInvoiceById(long invoiceId)
        {
            return _purchaseInvoiceRepository.GetInvoiceById(invoiceId);
        }

        public void AddInvoice(PurchaseInvoice purchaseInvoice)
        {
            _purchaseInvoiceRepository.AddInvoice(purchaseInvoice);
        }

        public void UpdateInvoice(PurchaseInvoice purchaseInvoice)
        {
            _purchaseInvoiceRepository.UpdateInvoice(purchaseInvoice);
        }

        public void DeleteInvoice(long invoiceId)
        {
            _purchaseInvoiceRepository.DeleteInvoice(invoiceId);
        }

        public IEnumerable<PurchaseInvoice> SearchInvoices(string keyword)
        {
            return _purchaseInvoiceRepository.SearchInvoices(keyword);
        }
    }
}
