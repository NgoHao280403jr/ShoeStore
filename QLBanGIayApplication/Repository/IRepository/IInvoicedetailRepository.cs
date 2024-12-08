using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application.Repository.IRepository
{
    public interface IInvoicedetailRepository
    {
        IEnumerable<Invoicedetail> GetAllInvoiceDetails();
        Invoicedetail GetInvoiceDetailById(long invoiceId, long productId);
        void AddInvoiceDetail(Invoicedetail invoiceDetail);
        void UpdateInvoiceDetail(Invoicedetail invoiceDetail);
        void DeleteInvoiceDetail(long invoiceId, long productId);
        void DeleteAllInvoiceDetailsByInvoiceId(long invoiceId);
        IEnumerable<Invoicedetail> GetInvoiceDetailsByInvoiceId(long invoiceId);
        IEnumerable<Invoicedetail> SearchInvoiceDetails(string keyword);
    }
}
