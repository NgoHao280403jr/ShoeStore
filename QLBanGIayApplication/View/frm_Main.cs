using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using QLBanGiay_Application.Repository;
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
    public partial class frm_Main : Form
    {
        private readonly UserService _userService;
        private bool isMenuOpen = false;
        public frm_Main(UserService userService)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.btn_Menu.Click += Btn_Menu_Click;
            this.btn_Qlsanpham.Click += Btn_Qlsanpham_Click;
            this.btn_Qldanhmuc.Click += Btn_Qldanhmuc_Click;
            this.btn_Qldanhmuc2.Click += Btn_Qldanhmuc2_Click;
            this.btn_Qlkhachhang.Click += Btn_Qlkhachhang_Click;
            this.btn_Qlnhanvien.Click += Btn_Qlnhanvien_Click;
            this.btn_Qlquyen.Click += Btn_Qlquyen_Click;
            this.btn_Qlnguoidung.Click += Btn_Qlnguoidung_Click;
            this.btn_Qlsize.Click += Btn_Qlsize_Click;
            this.btn_Qldonhang.Click += Btn_Qldonhang_Click;
            this.btn_Qlctdh.Click += Btn_Qlctdh_Click;
            this.btn_Qlhdbh.Click += Btn_Qlhdbh_Click;
            this.btn_Dangxuat.Click += Btn_Dangxuat_Click;
            _userService = userService;
            HideMenuButtons();
        }

        private void Btn_Dangxuat_Click(object? sender, EventArgs e)
        {
            this.Close();
            IUserRepository userRepository = new UserRepository(new QlShopBanGiayContext()); 
            UserService userService = new UserService(userRepository);
            frm_Login login = new frm_Login(userService);
            login.Show();
        }

        private void Btn_Qlhdbh_Click(object? sender, EventArgs e)
        {
            frm_Invoice mainForm = new frm_Invoice();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlctdh_Click(object? sender, EventArgs e)
        {
            //frm_OrderDetail mainForm = new frm_OrderDetail();
            //mainForm.Show();
            //Form parentForm = this.FindForm();
            //if (parentForm != null)
            //{
            //    parentForm.Hide();
             //}
        }

        private void Btn_Qldonhang_Click(object? sender, EventArgs e)
        {
            frm_Orders mainForm = new frm_Orders();
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlsize_Click(object? sender, EventArgs e)
        {
            frm_ProductSize mainForm = new frm_ProductSize(_userService);
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlnguoidung_Click(object? sender, EventArgs e)
        {
            string username = frm_Login.LoggedInUsername;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Bạn cần đăng nhập trước khi truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);

            if (user != null && user.Roleid == 1)
            {
                frm_Users usersForm = new frm_Users();
                usersForm.Show();
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide();
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Btn_Qlquyen_Click(object? sender, EventArgs e)
        {
            string username = frm_Login.LoggedInUsername;
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Bạn cần đăng nhập trước khi truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);

            if (user != null && user.Roleid == 1)
            {
                frm_Role roleForm = new frm_Role(_userService);
                roleForm.Show();
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide();
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Btn_Qlnhanvien_Click(object? sender, EventArgs e)
        {
            string username = frm_Login.LoggedInUsername; 
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Bạn cần đăng nhập trước khi truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);

            if (user != null && user.Roleid == 1) 
            {
                frm_Employee employeeForm = new frm_Employee(_userService);
                employeeForm.Show();
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.Hide();
                }
            }
            else
            {
                MessageBox.Show("Bạn không có quyền truy cập vào chức năng này!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Btn_Qlkhachhang_Click(object? sender, EventArgs e)
        {
            frm_Customers mainForm = new frm_Customers(_userService);
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qldanhmuc2_Click(object? sender, EventArgs e)
        {
            frm_ProductCategory mainForm = new frm_ProductCategory(_userService);
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qldanhmuc_Click(object? sender, EventArgs e)
        {
            frm_ParentProduct mainForm = new frm_ParentProduct(_userService);
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Qlsanpham_Click(object? sender, EventArgs e)
        {
            frm_Products mainForm = new frm_Products(_userService);
            mainForm.Show();
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                parentForm.Hide();
            }
        }

        private void Btn_Menu_Click(object? sender, EventArgs e)
        {
            if (isMenuOpen)
            {
                HideMenuButtons();
                isMenuOpen = false;
            }
            else
            {
                ShowMenuButtons();
                isMenuOpen = true;
            }
        }

        private void HideMenuButtons()
        {
            btn_Qldanhmuc.Visible = false;
            btn_Qldanhmuc2.Visible = false;
            btn_Qlsanpham.Visible = false;
            btn_Qlkhachhang.Visible = false;
            btn_Qlnguoidung.Visible = false;
            btn_Qlquyen.Visible = false;
            btn_Qlnhanvien.Visible = false;
            btn_Qldonhang.Visible = false;
            btn_Qlctdh.Visible = false;
            btn_Qlhdbh.Visible = false;
            btn_Qlsize.Visible = false;
        }
        private void ShowMenuButtons()
        {
            btn_Qldanhmuc.Visible = true;
            btn_Qldanhmuc2.Visible = true;
            btn_Qlsanpham.Visible = true;
            btn_Qlkhachhang.Visible = true;
            btn_Qlnguoidung.Visible = true;
            btn_Qlquyen.Visible = true;
            btn_Qlnhanvien.Visible = true;
            btn_Qldonhang.Visible = true;
            btn_Qlctdh.Visible = true;
            btn_Qlhdbh.Visible = true;
            btn_Qlsize.Visible = true;
        }

        private void frm_Main_Load(object sender, EventArgs e)
        {

        }
    }
}
