using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository;
using Microsoft.EntityFrameworkCore;
using QLBanGiay_Application.Services;
using Microsoft.Owin.BuilderProperties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Text.RegularExpressions;

namespace QLBanGiay_Application.View
{
    public partial class frm_Customers : Form
    {
        private readonly UserService _userService;
        private readonly CustomerService _customerService;
        private readonly QlShopBanGiayContext _context;
        private List<Customer> customer;
        public frm_Customers()
        {
            InitializeComponent();
            this.Load += Frm_Customers_Load;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Reset.Click += Btn_Reset_Click;
            this.dgv_danhsachkh.CellClick += Dgv_danhsachkh_CellClick;
            this.txt_Email.KeyPress += Txt_Email_KeyPress;
            this.txt_SDT.KeyPress += Txt_SDT_KeyPress;
            this.btn_Thoat.Click += Btn_Thoat_Click;

            _context = new QlShopBanGiayContext();
            _customerService = new CustomerService(new CustomerRepository(_context));
            _userService = new UserService(new UserRepository(_context));
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main();
            mainForm.Show();
        }

        private void Txt_SDT_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char inputChar = e.KeyChar;

            if (!char.IsDigit(inputChar) && inputChar != 8)
            {
                e.Handled = true;
            }

            if (txt_SDT.Text.Length >= 10 && inputChar != 8)
            {
                e.Handled = true;
            }
        }

        private void Txt_Email_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char inputChar = e.KeyChar;

            if (!char.IsLetterOrDigit(inputChar) && inputChar != 8 && inputChar != '.' && inputChar != '@' && inputChar != '_' && inputChar != '+' && inputChar != '-')
            {
                e.Handled = true;
            }
        }

        private void Btn_Reset_Click(object? sender, EventArgs e)
        {
            txt_TenTK.Enabled = true;
            txt_Makh.Text = "";
            txt_Timkiem.Text = "";
            txt_Tenkhachhang.Text = "";
            txt_DiaChi.Text = "";
            dtpBirthdate.Value = DateTime.Now;
            txt_Email.Text = "";
            txt_SDT.Text = "";
            txt_TenTK.Text = "";
            cmbGender.SelectedIndex = -1;
            LoadCustomer();
        }

        private void Dgv_danhsachkh_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedCustomer = (Customer)dgv_danhsachkh.Rows[e.RowIndex].DataBoundItem;
                var customer = _context.Customers.Include(c => c.User)
                                    .FirstOrDefault(c => c.Customerid == selectedCustomer.Customerid);

                txt_Makh.Text = selectedCustomer.Customerid.ToString();
                txt_Tenkhachhang.Text = selectedCustomer.Customername;
                txt_DiaChi.Text = selectedCustomer.Address;
                dtpBirthdate.Value = selectedCustomer.Birthdate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Now;
                txt_Email.Text = selectedCustomer.Email;
                txt_SDT.Text = selectedCustomer.Phonenumber;
                txt_TenTK.Text = customer.User.Username;
                cmbGender.SelectedItem = selectedCustomer.Gender;
                txt_TenTK.Enabled = false;
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Makh.Text) ||
             string.IsNullOrWhiteSpace(txt_SDT.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long customerId = long.Parse(txt_Makh.Text);
            var customer = _customerService.GetCustomerById(customerId);

            if (customer == null)
            {
                MessageBox.Show("Khách hàng không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            customer.Customername = txt_Tenkhachhang.Text.Trim();
            customer.Birthdate = DateOnly.FromDateTime(dtpBirthdate.Value);
            customer.Gender = (string?)cmbGender.SelectedItem;
            customer.Address = txt_DiaChi.Text;
            customer.Phonenumber = txt_SDT.Text;
            customer.Email = txt_Email.Text;

            try
            {
                _customerService.UpdateCustomer(customer);
                LoadCustomer();
                MessageBox.Show("Cập nhật khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Tenkhachhang.Text) || string.IsNullOrWhiteSpace(txt_SDT.Text) || string.IsNullOrWhiteSpace(txt_TenTK.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidEmail(txt_Email.Text))
            {
                MessageBox.Show("Email không hợp lệ. Vui lòng nhập lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool emailExists = _context.Employees.Any(u => u.Email == txt_Email.Text.Trim());
            if (emailExists)
            {
                MessageBox.Show("Email đã tồn tại. Vui lòng nhập email khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newCustomer = new Customer
            {
                Customername = txt_Tenkhachhang.Text.Trim(),
                Birthdate = DateOnly.FromDateTime(dtpBirthdate.Value),
                Gender = (string?)cmbGender.SelectedItem,
                Address = txt_DiaChi.Text,
                Phonenumber = txt_SDT.Text,
                Email = txt_Email.Text
            };
            var newUser = new User
            {
                Username = txt_TenTK.Text.Trim(),
                Password = "123",
                Roleid = 2,
                Isactive = true,
                Isbanned = false,
            };

            try
            {
                _userService.AddUser(newUser);

                newCustomer.Userid = newUser.Userid;

                _customerService.AddCustomer(newCustomer);

                LoadCustomer();
                MessageBox.Show("Thêm khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}\n{ex.InnerException?.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            var customer = _customerService.SearchCustomers(keyword);
            if (customer != null)
            {
                dgv_danhsachkh.DataSource = customer;
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng.");
            }
        }

        private void Frm_Customers_Load(object? sender, EventArgs e)
        {
            LoadCustomer();
            LoadGender();
        }
        private void LoadCustomer()
        {
            var customer = _customerService.GetAllCustomers();
            if (customer != null)
            {
                dgv_danhsachkh.DataSource = customer;
                dgv_danhsachkh.ReadOnly = true;

                dgv_danhsachkh.AutoGenerateColumns = false;
                dgv_danhsachkh.Columns.Clear();

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Customerid",
                    HeaderText = "Mã KH",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Customername",
                    HeaderText = "Tên KH",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Birthdate",
                    HeaderText = "Ngày Sinh",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Gender",
                    HeaderText = "Giới Tính",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });


                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Address",
                    HeaderText = "Địa Chỉ",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Phonenumber",
                    HeaderText = "Số Điện Thoại",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachkh.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachkh.ColumnHeadersHeight = 50;
                dgv_danhsachkh.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachkh.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dgv_danhsachkh.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachkh.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachkh.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachkh.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachkh.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có khách hàng nào để hiển thị.");
            }
        }
        private void LoadGender()
        {
            var genders = _context.Customers
                                     .Where(c => !string.IsNullOrEmpty(c.Gender))
                                     .Select(c => c.Gender)
                                     .Distinct()
                                     .ToList();

            cmbGender.DataSource = genders;
            cmbGender.SelectedIndex = -1;
        }
        private bool IsValidEmail(string email)
        {
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return emailRegex.IsMatch(email);
        }
    }
}
