using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay.Models.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;

namespace QLBanGiay_Application.Repository
{
    public class InvoicedetailRepository : IInvoicedetailRepository
    {
        private readonly QlShopBanGiayContext _context;

        public InvoicedetailRepository(QlShopBanGiayContext context)
        {
            _context = context;
        }

        public IEnumerable<Invoicedetail> GetAllInvoiceDetails()
        {
            return _context.Invoicedetails.ToList();
        }

        public Invoicedetail GetInvoiceDetailById(long invoiceId, long productId)
        {
            return _context.Invoicedetails.FirstOrDefault(id => id.Invoiceid == invoiceId && id.Productid == productId);
        }

        public void AddInvoiceDetail(Invoicedetail invoiceDetail)
        {
            _context.Invoicedetails.Add(invoiceDetail);
            _context.SaveChanges();
        }

        public void UpdateInvoiceDetail(Invoicedetail invoiceDetail)
        {
            _context.Invoicedetails.Update(invoiceDetail);
            _context.SaveChanges();
        }

        public void DeleteInvoiceDetail(long invoiceId, long productId)
        {
            var invoiceDetail = GetInvoiceDetailById(invoiceId, productId);
            if (invoiceDetail != null)
            {
                _context.Invoicedetails.Remove(invoiceDetail);
                _context.SaveChanges();
            }
        }
        public void DeleteAllInvoiceDetailsByInvoiceId(long invoiceId)
        {
            var invoiceDetails = _context.Invoicedetails.Where(id => id.Invoiceid == invoiceId).ToList();
            if (invoiceDetails.Any())
            {
                _context.Invoicedetails.RemoveRange(invoiceDetails);
                _context.SaveChanges();
            }
        }
        public IEnumerable<Invoicedetail> GetInvoiceDetailsByInvoiceId(long invoiceId)
        {
            return _context.Invoicedetails.Where(id => id.Invoiceid == invoiceId).ToList();
        }

        public IEnumerable<Invoicedetail> SearchInvoiceDetails(string keyword)
        {
            return _context.Invoicedetails.Where(id => id.Product.Productname.Contains(keyword)).ToList();
        }
    }
}
