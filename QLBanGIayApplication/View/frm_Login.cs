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

            // Kiểm tra thông tin đăng nhập
            if (_userService.ValidateLogin(username, password))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                this.Hide();

                frm_Main mainForm = new frm_Main();
                mainForm.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
