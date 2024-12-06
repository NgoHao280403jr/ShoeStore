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
    public partial class frm_Login : Form
    {
        private readonly UserService _userService;
        public frm_Login(UserService userService)
        {
            InitializeComponent();
            btn_DangNhap.Click += Btn_DangNhap_Click;
            _userService = userService;
        }

        private void Btn_DangNhap_Click(object? sender, EventArgs e)
        {
            string username = txt_UserName.Text.Trim();
            string password = txt_PassWord.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (_userService.ValidateLogin(username, password))
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    frmMain mainForm = new frmMain();
                    frm_ProductSize mainForm = new frm_ProductSize();
                    mainForm.Show();
                }
                else
                {

                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ckHienThi_CheckedChanged(object sender, EventArgs e)
        {          
            if (ckHienThi.Checked)
            {
                txt_PassWord.PasswordChar = '\0';
            }
            else
            {
                txt_PassWord.PasswordChar = '*'; 
            }
        }
    }
}
