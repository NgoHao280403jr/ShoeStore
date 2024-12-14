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
            cbb_Size.DisplayMember = "ProductSizeId";   
            cbb_Size.ValueMember = "ProductSizeId";
        }

        private void LoadDataGridView()
        {
            try
            {
                // Lấy danh sách hóa đơn nhập hàng chỉ với các thuộc tính cơ bản
                var invoices = _purchaseInvoiceService.GetAllInvoices()
                    .Select(invoice => new
                    {
                        InvoiceId = invoice.InvoiceId,
                        ProductId = invoice.ProductId,
                        ProductSizeId = invoice.ProductSizeId,
                        Quantity = invoice.Quantity,
                        UnitPrice = invoice.UnitPrice,
                        TotalPrice = invoice.TotalPrice,
                        ImportDate = invoice.ImportDate.ToString("dd/MM/yyyy"),
                        EmployeeId = invoice.EmployeeId
                    })
                    .ToList();

                // Gán dữ liệu vào DataGridView
                dgv_PurchaseInvoices.DataSource = invoices;

                // Tùy chỉnh tên cột hiển thị trong DataGridView
                dgv_PurchaseInvoices.Columns["InvoiceId"].HeaderText = "Mã Hóa Đơn";
                dgv_PurchaseInvoices.Columns["ProductId"].HeaderText = "Mã Sản Phẩm";
                dgv_PurchaseInvoices.Columns["ProductSizeId"].HeaderText = "Mã Kích Cỡ";
                dgv_PurchaseInvoices.Columns["Quantity"].HeaderText = "Số Lượng";
                dgv_PurchaseInvoices.Columns["UnitPrice"].HeaderText = "Đơn Giá";
                dgv_PurchaseInvoices.Columns["TotalPrice"].HeaderText = "Thành Tiền";
                dgv_PurchaseInvoices.Columns["ImportDate"].HeaderText = "Ngày Nhập";
                dgv_PurchaseInvoices.Columns["EmployeeId"].HeaderText = "Mã Nhân Viên";

                // Đảm bảo DataGridView không tự động tạo cột
                dgv_PurchaseInvoices.AutoGenerateColumns = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_hdnhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRow = dgv_PurchaseInvoices.Rows[e.RowIndex];

                txt_InvoiceId.Text = selectedRow.Cells["InvoiceId"].Value?.ToString();
                cbo_Product.SelectedValue = Convert.ToInt64(selectedRow.Cells["ProductId"].Value);
                cbo_Employee.SelectedValue = Convert.ToInt64(selectedRow.Cells["EmployeeId"].Value);
                txt_Quantity.Text = selectedRow.Cells["Quantity"].Value?.ToString();
                txt_UnitPrice.Text = selectedRow.Cells["UnitPrice"].Value?.ToString();
                txt_TotalPrice.Text = selectedRow.Cells["TotalPrice"].Value?.ToString();

                if (DateTime.TryParse(selectedRow.Cells["ImportDate"].Value?.ToString(), out var importDate))
                {
                    dtp_ImportDate.Value = importDate;
                }

                cbb_Size.SelectedValue = Convert.ToInt64(selectedRow.Cells["ProductSizeId"].Value);
            }
        }
        private void btn_Them_Click(object sender, EventArgs e)
        {
            try
            {
                var productSizeId = Convert.ToInt64(cbb_Size.SelectedValue);

                var invoice = new PurchaseInvoice
                {
                    ProductId = (long)cbo_Product.SelectedValue,
                    ProductSizeId = productSizeId, 
                    EmployeeId = (long)cbo_Employee.SelectedValue,
                    Quantity = int.Parse(txt_Quantity.Text),
                    UnitPrice = double.Parse(txt_UnitPrice.Text),
                    TotalPrice = double.Parse(txt_TotalPrice.Text),
                    ImportDate = DateTime.SpecifyKind(dtp_ImportDate.Value, DateTimeKind.Utc),
                };

                var productsize = _productSizeService.GetProductSizeById(productSizeId); 
                _purchaseInvoiceService.AddInvoice(invoice);

                _productSizeService.UpdateProductSizeQuantity(invoice.ProductId, productsize.Size, invoice.Quantity);

                MessageBox.Show("Hàng đã được thêm thành công.");
                LoadDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi nhập hàng: {ex}");
            }
        }

        private void btn_Capnhat_Click(object sender, EventArgs e)
        {
            txt_InvoiceId.Text = "";
            txt_Quantity.Text = "";
            txt_UnitPrice.Text = "";
            txt_TotalPrice.Text = "";
            LoadComboBox();
            dtp_ImportDate.Value = DateTime.Now;
            //try
            //{
            //    var invoiceId = long.Parse(txt_InvoiceId.Text);
            //    var oldInvoice = _purchaseInvoiceService.GetInvoiceById(invoiceId);

            //    MessageBox.Show(invoiceId+ "");
            //    if (oldInvoice == null)
            //    {
            //        MessageBox.Show("Hóa đơn không tồn tại.");
            //        return;
            //    }

            //    // Kiểm tra giá trị của oldInvoice trước khi tạo hóa đơn mới
            //    var productSizeId = Convert.ToInt64(cbb_Size.SelectedValue);

            //    var invoice = new PurchaseInvoice
            //    {
            //        InvoiceId = invoiceId,
            //        ProductId = (long)cbo_Product.SelectedValue,
            //        ProductSizeId = productSizeId,
            //        EmployeeId = (long)cbo_Employee.SelectedValue,
            //        Quantity = int.Parse(txt_Quantity.Text), // Giá trị nhập từ textbox
            //        UnitPrice = double.Parse(txt_UnitPrice.Text),
            //        TotalPrice = double.Parse(txt_TotalPrice.Text),
            //        ImportDate = DateTime.SpecifyKind(dtp_ImportDate.Value, DateTimeKind.Utc),
            //    };

            //    _purchaseInvoiceService.UpdateInvoice(invoice);
            //    MessageBox.Show(oldInvoice.Quantity + "");
            //    var quantityDifference = invoice.Quantity - oldInvoice.Quantity;

            //    var productsize = _productSizeService.GetProductSizeById(productSizeId);
            //    _productSizeService.UpdateProductSizeQuantity(invoice.ProductId, productsize.Size, quantityDifference);

            //    MessageBox.Show("Hóa đơn đã được cập nhật thành công.");
            //    LoadDataGridView();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Lỗi khi cập nhật hóa đơn: {ex.Message}");
            //}
        }


        private void btn_Xoa_Click(object sender, EventArgs e)
        {
            try
            {
                var invoiceId = long.Parse(txt_InvoiceId.Text);
                var oldInvoice = _purchaseInvoiceService.GetInvoiceById(invoiceId);
                var productSizeId = Convert.ToInt64(cbb_Size.SelectedValue);

                if (oldInvoice == null)
                {
                    MessageBox.Show("Hóa đơn không tồn tại.");
                    return;
                }

                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa hóa đơn này?",
                                                    "Xác nhận xóa",
                                                    MessageBoxButtons.YesNo);

                if (confirmResult == DialogResult.Yes)
                {
                    var productsize = _productSizeService.GetProductSizeById(productSizeId);

                    _purchaseInvoiceService.DeleteInvoice(invoiceId);

                    _productSizeService.UpdateProductSizeQuantity(oldInvoice.ProductId, productsize.Size, -oldInvoice.Quantity);

                    MessageBox.Show("Hóa đơn đã được xóa thành công.");
                    LoadDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex}");
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
