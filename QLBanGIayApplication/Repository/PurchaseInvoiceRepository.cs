using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Repository
{
    public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
    {
        private readonly QlShopBanGiayContext _context;

        public PurchaseInvoiceRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }
        public IEnumerable<PurchaseInvoice> GetAllInvoices()
        {
            return _context.PurchaseInvoices.ToList();
        }

        public PurchaseInvoice GetInvoiceById(long invoiceId)
        {
            return _context.PurchaseInvoices.FirstOrDefault(i => i.InvoiceId == invoiceId);
        }

        public void AddInvoice(PurchaseInvoice purchaseInvoice)
        {
            _context.PurchaseInvoices.Add(purchaseInvoice);
            _context.SaveChanges();
        }

        public void UpdateInvoice(PurchaseInvoice purchaseInvoice)
        {
            try
            {
                var existingInvoice = _context.PurchaseInvoices
                                              .FirstOrDefault(p => p.InvoiceId == purchaseInvoice.InvoiceId);

                if (existingInvoice != null)
                {
                    _context.Entry(existingInvoice).CurrentValues.SetValues(purchaseInvoice);
                }
                else
                {
                    _context.PurchaseInvoices.Update(purchaseInvoice);
                }
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi cập nhật hóa đơn mua hàng: " + ex.Message);
            }
        }

        public void DeleteInvoice(long invoiceId)
        {
            var invoice = GetInvoiceById(invoiceId);
            if (invoice != null)
            {
                _context.PurchaseInvoices.Remove(invoice);
                _context.SaveChanges();
            }
        }

        public IEnumerable<PurchaseInvoice> SearchInvoices(string keyword)
        {
            return _context.PurchaseInvoices
                .Where(i => i.Employee.Employeename.Contains(keyword) || i.Product.Productname.Contains(keyword))
                .ToList();
        }
    }
}
