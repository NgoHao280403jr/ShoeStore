using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay_Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLBanGiay_Application.View
{
    public partial class frm_PurchaseInvoice : Form
    {
        private readonly ProductService _productService;
        private readonly ProductSizeService _productSizeService;
        private readonly PurchaseInvoiceService _purchaseInvoiceService;
        private readonly EmployeeService _employeeService;
        private readonly QlShopBanGiayContext _context;
        private readonly UserService _userService;
        public frm_PurchaseInvoice()
        {
            InitializeComponent();
            _context = new QlShopBanGiayContext();
            _productService = new ProductService(new ProductRepository(_context));
            _productSizeService = new ProductSizeService(new ProductSizeRepository(_context));
            _purchaseInvoiceService = new PurchaseInvoiceService(new PurchaseInvoiceRepository(_context));
            _employeeService = new EmployeeService(new EmployeeRepository(_context));
            _userService = new UserService(new UserRepository(_context));
        }
        private void frm_PurchaseInvoice_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            LoadDataGridView();
        }
        private void LoadComboBox()
        {
            var products = _productService.GetAllProducts();
            cbo_Product.DataSource = products;
            cbo_Product.DisplayMember = "ProductName";
            cbo_Product.ValueMember = "ProductId";

            var employees = _employeeService.GetAllEmployees();
            cbo_Employee.DataSource = employees;
            cbo_Employee.DisplayMember = "EmployeeName";
            cbo_Employee.ValueMember = "EmployeeId";

            var size = _productSizeService.GetAllProductSizes();
            cbb_Size.DataSource = size;
            cbb_Size.DisplayMember = "Size";
            cbb_Size.ValueMember = "ProductSizeId";
        }

        private void LoadDataGridView()
        {
            var invoices = _purchaseInvoiceService.GetAllInvoices();
            dgv_PurchaseInvoices.DataSource = invoices;
        }
        private void dgv_hdnhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedInvoice = (PurchaseInvoice)dgv_PurchaseInvoices.Rows[e.RowIndex].DataBoundItem;

                txt_InvoiceId.Text = selectedInvoice.InvoiceId.ToString();
                cbo_Product.SelectedValue = selectedInvoice.ProductId;
                cbo_Employee.SelectedValue = selectedInvoice.EmployeeId;
                txt_Quantity.Text = selectedInvoice.Quantity.ToString();
                txt_UnitPrice.Text = selectedInvoice.UnitPrice.ToString();
                txt_TotalPrice.Text = selectedInvoice.TotalPrice.ToString();
                dtp_ImportDate.Value = selectedInvoice.ImportDate;
                cbb_Size.SelectedValue = selectedInvoice.ProductSizeId;
            }
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            try
            {
                var invoice = new PurchaseInvoice
                {
                    ProductId = (long)cbo_Product.SelectedValue,
                    EmployeeId = (long)cbo_Employee.SelectedValue,
                    Quantity = int.Parse(txt_Quantity.Text),
                    UnitPrice = double.Parse(txt_UnitPrice.Text),
                    TotalPrice = double.Parse(txt_TotalPrice.Text),
                    ImportDate = dtp_ImportDate.Value
                };

                _purchaseInvoiceService.AddInvoice(invoice);
                //_productSizeService.UpdateProductSizeQuantity(invoice.ProductId, invoice.Quantity);

                MessageBox.Show("Hóa đơn đã được thêm thành công.");
                LoadDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}");
            }
        }

        private void btn_Capnhat_Click(object sender, EventArgs e)
        {
            try
            {
                var invoiceId = long.Parse(txt_InvoiceId.Text);
                var oldInvoice = _purchaseInvoiceService.GetInvoiceById(invoiceId);

                if (oldInvoice == null)
                {
                    MessageBox.Show("Hóa đơn không tồn tại.");
                    return;
                }

                var invoice = new PurchaseInvoice
                {
                    InvoiceId = invoiceId,
                    ProductId = (long)cbo_Product.SelectedValue,
                    EmployeeId = (long)cbo_Employee.SelectedValue,
                    Quantity = int.Parse(txt_Quantity.Text),
                    UnitPrice = double.Parse(txt_UnitPrice.Text),
                    TotalPrice = double.Parse(txt_TotalPrice.Text),
                    ImportDate = dtp_ImportDate.Value
                };

                _purchaseInvoiceService.UpdateInvoice(invoice);

                // Tính toán sự thay đổi số lượng và cập nhật
                var quantityDifference = invoice.Quantity - oldInvoice.Quantity;
                //_productSizeService.UpdateProductSizeQuantity(invoice.ProductId, quantityDifference);

                MessageBox.Show("Hóa đơn đã được cập nhật thành công.");
                LoadDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hóa đơn: {ex.Message}");
            }
        }

        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                var invoiceId = long.Parse(txt_InvoiceId.Text);
                var oldInvoice = _purchaseInvoiceService.GetInvoiceById(invoiceId);

                if (oldInvoice == null)
                {
                    MessageBox.Show("Hóa đơn không tồn tại.");
                    return;
                }

                // Hiển thị thông báo xác nhận trước khi xóa
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    _purchaseInvoiceService.DeleteInvoice(invoiceId);
                    //_productSizeService.UpdateProductSizeQuantity(oldInvoice.ProductId, -oldInvoice.Quantity);

                    MessageBox.Show("Hóa đơn đã được xóa thành công.");
                    LoadDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}");
            }
        }


        private void btn_Thoat_Click(object sender, EventArgs e)
        {
            frm_Main main = new frm_Main(_userService);
            this.Close();
            main.ShowDialog();
        }
    }
}
