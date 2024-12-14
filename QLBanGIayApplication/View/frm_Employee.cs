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
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace QLBanGiay_Application.View
{
    public partial class frm_Employee : Form
    {
        private readonly UserService _userService;
        private readonly EmployeeService _employeeService;
        private readonly QlShopBanGiayContext _context;
        private List<Employee> customer;
        public frm_Employee(UserService userService)
        {
            InitializeComponent();

            this.Load += Frm_Employee_Load;
            this.btn_TimKiem.Click += Btn_TimKiem_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Them.Click += Btn_Them_Click;
            this.btn_Reset.Click += Btn_Reset_Click;
            this.dgv_danhsachnv.CellClick += Dgv_danhsachnv_CellClick;
            this.txt_Email.KeyPress += Txt_Email_KeyPress;
            this.txt_SDT.KeyPress += Txt_SDT_KeyPress;

            _context = new QlShopBanGiayContext();
            _employeeService = new EmployeeService(new EmployeeRepository(_context));
            _userService = new UserService(new UserRepository(_context));
            _userService = userService;
        }

        private void Txt_SDT_KeyPress(object? sender, KeyPressEventArgs e)
        {
            char inputChar = e.KeyChar;

            if (!char.IsDigit(inputChar) && inputChar != 8)
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

        private void Dgv_danhsachnv_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedEmployee = (Employee)dgv_danhsachnv.Rows[e.RowIndex].DataBoundItem;
                var employee = _context.Employees.Include(c => c.User)
                                    .FirstOrDefault(c => c.Employeeid == selectedEmployee.Employeeid);
                txt_Manhanvien.Text = selectedEmployee.Employeeid.ToString();
                txt_Tennhanvien.Text = selectedEmployee.Employeename;
                txt_DiaChi.Text = selectedEmployee.Address;
                dtpBirthdate.Value = selectedEmployee.Birthdate?.ToDateTime(TimeOnly.MinValue) ?? DateTime.Now;
                txt_Email.Text = selectedEmployee.Email;
                txt_SDT.Text = selectedEmployee.Phonenumber;
                txt_TenTK.Text = employee.User.Username;
                cmbGender.SelectedItem = selectedEmployee.Gender;
                txt_TenTK.Enabled = false;
            }
        }

        private void Btn_Reset_Click(object? sender, EventArgs e)
        {
            txt_TenTK.Enabled = true;
            txt_Manhanvien.Text = "";
            txt_Timkiem.Text = "";
            txt_Tennhanvien.Text = "";
            txt_DiaChi.Text = "";
            dtpBirthdate.Value = DateTime.Now;
            txt_Email.Text = "";
            txt_SDT.Text = "";
            txt_TenTK.Text = "";
            cmbGender.SelectedIndex = -1;
            LoadEmployee();
        }

        private void Btn_Them_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Tennhanvien.Text) || string.IsNullOrWhiteSpace(txt_SDT.Text) || string.IsNullOrWhiteSpace(txt_TenTK.Text))
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

            var newEmployee = new Employee
            {
                Employeename = txt_Tennhanvien.Text.Trim(),
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
                Roleid = 1,
                Isactive = true,
                Isbanned = false,
            };

            bool usernameExists = _context.Users.Any(u => u.Username == txt_Tennhanvien.Text.Trim());
            if (usernameExists)
            {
                MessageBox.Show("Username đã tồn tại. Vui lòng nhập username khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool customerExists = _context.Customers.Any(c => c.Phonenumber == txt_SDT.Text.Trim());
            if (customerExists)
            {
                MessageBox.Show("Số điện thoại đã tồn tại. Vui lòng nhập số khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _userService.AddUser(newUser);

                newEmployee.Userid = newUser.Userid;

                _employeeService.AddEmployee(newEmployee);

                LoadEmployee();
                MessageBox.Show("Thêm khách hàng thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}\n{ex.InnerException?.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Manhanvien.Text) ||
             string.IsNullOrWhiteSpace(txt_Tennhanvien.Text))
            {
                MessageBox.Show("Vui lòng nhập tất cả thông tin bắt buộc.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long employeeId = long.Parse(txt_Manhanvien.Text);
            var employee = _employeeService.GetEmployeeById(employeeId);

            if (employee == null)
            {
                MessageBox.Show("Nhân viên không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            employee.Employeename = txt_Tennhanvien.Text.Trim();
            employee.Birthdate = DateOnly.FromDateTime(dtpBirthdate.Value);
            employee.Gender = (string?)cmbGender.SelectedItem;
            employee.Address = txt_DiaChi.Text;
            employee.Phonenumber = txt_SDT.Text;
            employee.Email = txt_Email.Text;

            try
            {
                _employeeService.UpdateEmployee(employee);
                LoadEmployee();
                MessageBox.Show("Cập nhật nhân viên thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật nhân viên: {ex.Message}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_TimKiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim().ToLower();

            var customer = _employeeService.SearchEmployees(keyword);
            if (customer != null)
            {
                dgv_danhsachnv.DataSource = customer;
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng.");
            }
        }

        private void Frm_Employee_Load(object? sender, EventArgs e)
        {
            LoadEmployee();
            LoadGender();
        }
        private void LoadEmployee()
        {
            var customer = _employeeService.GetAllEmployees();
            if (customer != null)
            {
                dgv_danhsachnv.DataSource = customer;
                dgv_danhsachnv.ReadOnly = true;

                dgv_danhsachnv.AutoGenerateColumns = false;
                dgv_danhsachnv.Columns.Clear();

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Employeeid",
                    HeaderText = "Mã NV",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Employeename",
                    HeaderText = "Tên NV",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Birthdate",
                    HeaderText = "Ngày Sinh",
                    Width = 150,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Gender",
                    HeaderText = "Giới Tính",
                    Width = 50,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });


                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Address",
                    HeaderText = "Địa Chỉ",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Phonenumber",
                    HeaderText = "Số Điện Thoại",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
                });

                dgv_danhsachnv.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Email",
                    HeaderText = "Email",
                    Width = 200,
                    DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleLeft }
                });

                dgv_danhsachnv.ColumnHeadersHeight = 50;
                dgv_danhsachnv.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachnv.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
                dgv_danhsachnv.DefaultCellStyle.Font = new Font("Arial", 10);

                dgv_danhsachnv.ColumnHeadersDefaultCellStyle.BackColor = Color.LightGreen;
                dgv_danhsachnv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv_danhsachnv.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 11, FontStyle.Bold);
                dgv_danhsachnv.EnableHeadersVisualStyles = false;
            }
            else
            {
                MessageBox.Show("Không có khách hàng nào để hiển thị.");
            }
        }
        private void LoadGender()
        {
            var genders = _context.Employees
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
