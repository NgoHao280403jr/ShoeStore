using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLBanGiay.Models.Models
{
    public partial class PurchaseInvoice
    {
        public long InvoiceId { get; set; }

        public long ProductId { get; set; }
        public long ProductSizeId { get; set; }
        public int Quantity { get; set; }

        public double UnitPrice { get; set; }

        public double TotalPrice { get; set; }

        public DateTime ImportDate { get; set; } = DateTime.Now;

        public long EmployeeId { get; set; }

        public virtual Product Product { get; set; } = null!;

        public virtual Employee Employee { get; set; } = null!;
        public virtual ProductSize ProductSize { get; set; } = null!;

    }
}
