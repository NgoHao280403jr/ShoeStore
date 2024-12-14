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
            try
            {
                var productExists = _context.Products.Any(p => p.Productid == purchaseInvoice.ProductId);
                var productSizeExists = _context.ProductSizes.Any(ps => ps.ProductSizeId == purchaseInvoice.ProductSizeId);

                if (!productExists || !productSizeExists)
                {
                    throw new Exception("Sản phẩm hoặc kích thước sản phẩm không tồn tại.");
                }

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
                // Kiểm tra và ghi lại lỗi chi tiết
                if (ex.InnerException != null)
                {
                    throw new Exception("Lỗi khi cập nhật hóa đơn mua hàng: " + ex.InnerException.Message);
                }
                else
                {
                    throw new Exception("Lỗi khi cập nhật hóa đơn mua hàng: " + ex.Message);
                }
            }
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
                if (ex.InnerException != null)
                {
                    throw new Exception("Lỗi khi cập nhật hóa đơn mua hàng: " + ex.InnerException.Message);
                }
                else
                {
                    throw new Exception("Lỗi khi cập nhật hóa đơn mua hàng: " + ex.Message);
                }
            }
        }

        public void DeleteInvoice(long invoiceId)
        {
            try
            {
                var invoice = _context.PurchaseInvoices.FirstOrDefault(p => p.InvoiceId == invoiceId);

                if (invoice != null)
                {
                    _context.PurchaseInvoices.Remove(invoice);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Hóa đơn không tồn tại.");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    throw new Exception("Lỗi khi xóa hóa đơn mua hàng: " + ex.InnerException.Message);
                }
                else
                {
                    throw new Exception("Lỗi khi xóa hóa đơn mua hàng: " + ex.Message);
                }
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
