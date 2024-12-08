using QLBanGiay.Models.Models;
using QLBanGiay_Application.Services;
using QLBanGiay_Application.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;

namespace QLBanGiay_Application.View
{
    public partial class frm_Invoice : Form
    {
        private readonly InvoiceService _invoiceService;
        private readonly EmployeeService _employeeService;
        private readonly InvoicedetailService _invoicedetailService;
        private readonly QlShopBanGiayContext _context;
        private List<Invoice> invoice;
        public frm_Invoice()
        {
            InitializeComponent();

            this.Load += Frm_Invoice_Load;
            this.btn_TimKiem.Click += Btn_TimKiem_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.dgv_danhsachbh.CellDoubleClick += Dgv_danhsachbh_CellDoubleClick;
            this.btn_Reset.Click += Btn_Reset_Click;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.dgv_danhsachbh.CellClick += Dgv_danhsachbh_CellClick;
            this.txtPhone.KeyPress += TxtPhone_KeyPress;

            _context = new QlShopBanGiayContext();
            _employeeService = new EmployeeService(new EmployeeRepository(_context));
            _invoiceService = new InvoiceService(new InvoiceRepository(_context));
            _invoicedetailService = new InvoicedetailService(new InvoicedetailRepository(_context));
        }

        private void Dgv_danhsachbh_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Madm.Text))
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long invoiceId = long.Parse(txt_Madm.Text);

            try
            {
                
                var existinginvoice = _invoiceService.GetInvoiceById(invoiceId);
                if (existinginvoice == null)
                {
                    MessageBox.Show("Hóa đơn không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var invoiceDetails = _invoicedetailService.GetInvoiceDetailsByInvoiceId(invoiceId);
                if (invoiceDetails != null && invoiceDetails.Any())
                {
                    _invoicedetailService.DeleteAllInvoiceDetailsByInvoiceId(invoiceId);
                }
                _invoiceService.DeleteInvoice(invoiceId);
                LoadInvoice();
                MessageBox.Show("Xóa hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa hóa đơn: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TxtPhone_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char inputChar = e.KeyChar;

            if (!char.IsDigit(inputChar) && inputChar != 8)
            {
                e.Handled = true;
            }
        }

        private void Dgv_danhsachbh_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedInvoice = (Invoice)dgv_danhsachbh.Rows[e.RowIndex].DataBoundItem;
                var employee = _context.Invoices.Include(c => c.Employee)
                                    .FirstOrDefault(c => c.Employeeid == selectedInvoice.Employeeid);

                txt_Madm.Text = selectedInvoice.Invoiceid.ToString();
                dtpIssuedate.Value = selectedInvoice.Issuedate;
                txtPhone.Text = selectedInvoice.Phonenumber;
                cbb_nv.SelectedItem = employee.Employee.Employeename;
                cbo_Madmcha.SelectedItem = selectedInvoice.Paymentmethod;
            }
        }

        private void Btn_Reset_Click(object? sender, EventArgs e)
        {
            txt_Madm.Text = "";
            txt_Timkiem.Text = "";
            cbb_nv.SelectedIndex = -1;
            cbo_Madmcha.SelectedIndex = -1;
            txtPhone.Text = "";
            dtpIssuedate.Value = DateTime.Now;
            LoadInvoice();
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!long.TryParse(txt_Madm.Text, out long invoiceId))
            {
                MessageBox.Show("Mã hóa đơn không hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var invoice = _invoiceService.GetInvoiceById(invoiceId);

            if (invoice == null)
            {
                MessageBox.Show("Hóa đơn không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbb_nv.SelectedValue == null || !long.TryParse(cbb_nv.SelectedValue.ToString(), out long employeeId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            invoice.Employeeid = employeeId;
            invoice.Issuedate = dtpIssuedate.Value;
            invoice.Paymentmethod = (string?)cbo_Madmcha.SelectedItem;
            invoice.Phonenumber = txtPhone.Text;

            try
            {
                _invoiceService.UpdateInvoice(invoice);
                LoadInvoice();
                MessageBox.Show("Cập nhật hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật hóa đơn:{ex.Message}\n{ex.InnerException?.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPhone.Text) || string.IsNullOrWhiteSpace((string?)cbo_Madmcha.SelectedItem))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbb_nv.SelectedValue == null || !long.TryParse(cbb_nv.SelectedValue.ToString(), out long employeeId))
            {
                MessageBox.Show("Vui lòng chọn nhân viên hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newInvoice = new Invoice
            {
                Employeeid = employeeId,
                Issuedate = dtpIssuedate.Value != DateTime.MinValue ? dtpIssuedate.Value : DateTime.Now,
                Paymentmethod = (string?)cbo_Madmcha.SelectedItem,
                Phonenumber = txtPhone.Text,
            };

            try
            {
                _invoiceService.AddInvoice(newInvoice);

                LoadInvoice();
                MessageBox.Show("Thêm hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm hóa đơn: {ex.Message}\n{ex.InnerException?.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_TimKiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            var invoice = _invoiceService.SearchInvoices(keyword);
            if (invoice != null)
            {
                dgv_danhsachbh.DataSource = invoice;
            }
            else
            {
                MessageBox.Show("Không tìm thấy hóa đơn.");
            }
        }

        private void Frm_Invoice_Load(object? sender, EventArgs e)
        {
            LoadInvoice();
            LoadPaymentmethod();
            LoadEmployee();
        }
        private void LoadInvoice()
        {
            var invoice = _invoiceService.GetAllInvoices();
            if (invoice != null)
            {
                dgv_danhsachbh.DataSource = invoice;
                dgv_danhsachbh.ReadOnly = true;

                dgv_danhsachbh.AutoGenerateColumns = false;
                dgv_danhsachbh.Columns.Clear();

                dgv_danhsachbh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Invoiceid",
                    HeaderText = "Mã HĐ",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachbh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Phonenumber",
                    HeaderText = "Số Điện Thoại",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachbh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Paymentmethod",
                    HeaderText = "Phương Thức Thanh Toán",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });


                dgv_danhsachbh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Issuedate",
                    HeaderText = "Ngày Xuất HĐ",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachbh.ColumnHeadersHeight = 50;
                dgv_danhsachbh.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachbh.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dgv_danhsachbh.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachbh.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachbh.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachbh.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachbh.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có hóa đơn nào để hiển thị.");
            }
        }
        private void LoadPaymentmethod()
        {
            var paymentmethod = _context.Invoices.Select(c => c.Paymentmethod)
                                                 .Distinct()
                                                 .ToList();

            cbo_Madmcha.DataSource = paymentmethod;
            cbo_Madmcha.SelectedIndex = -1;
        }
        private void LoadEmployee()
        {
            var employees = _context.Employees
                                    .Select(e => new { e.Employeeid, e.Employeename })
                                    .Distinct()
                                    .ToList();

            cbb_nv.DataSource = employees;
            cbb_nv.DisplayMember = "Employeename"; 
            cbb_nv.ValueMember = "Employeeid";
        }
    }
}
