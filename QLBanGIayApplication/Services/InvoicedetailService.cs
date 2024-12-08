using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay_Application.Services
{
    public class InvoicedetailService
    {
        private readonly IInvoicedetailRepository _invoicedetailrepository;

        public InvoicedetailService(IInvoicedetailRepository invoicedetailrepository)
        {
            _invoicedetailrepository = invoicedetailrepository;
        }

        public IEnumerable<Invoicedetail> GetAllInvoiceDetails()
        {
            return _invoicedetailrepository.GetAllInvoiceDetails();
        }

        public Invoicedetail GetInvoiceDetailById(long invoiceId, long productId)
        {
            return _invoicedetailrepository.GetInvoiceDetailById(invoiceId, productId);
        }

        public void AddInvoiceDetail(Invoicedetail invoiceDetail)
        {
            _invoicedetailrepository.AddInvoiceDetail(invoiceDetail);
        }

        public void UpdateInvoiceDetail(Invoicedetail invoiceDetail)
        {
            _invoicedetailrepository.UpdateInvoiceDetail(invoiceDetail);
        }

        public void DeleteInvoiceDetail(long invoiceId, long productId)
        {
            _invoicedetailrepository.DeleteInvoiceDetail(invoiceId, productId);
        }
        public void DeleteAllInvoiceDetailsByInvoiceId(long invoiceId)
        {
            _invoicedetailrepository.DeleteAllInvoiceDetailsByInvoiceId(invoiceId);
        }

        public IEnumerable<Invoicedetail> GetInvoiceDetailsByInvoiceId(long invoiceId)
        {
            return _invoicedetailrepository.GetInvoiceDetailsByInvoiceId(invoiceId);
        }

        public IEnumerable<Invoicedetail> SearchInvoiceDetails(string keyword)
        {
            return _invoicedetailrepository.SearchInvoiceDetails(keyword);
        }
    }
}
