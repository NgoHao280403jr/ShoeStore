using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QLBanGiay.Models;
using QLBanGiay.Models.Models;

namespace QLBanGiay_Application
{
    public partial class frm_Login : Form
    {
        public frm_Login()
        {
            InitializeComponent();
            btn_DangNhap.Click += Btn_DangNhap_Click;
        }

        private void Btn_DangNhap_Click(object? sender, EventArgs e)
        {
            string username = txt_UserName.Text.Trim();
            string password = txt_PassWord.Text.Trim();

            // Kiểm tra thông tin đăng nhập
            if (IsLoginValid(username, password))
            {
                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Thực hiện các hành động sau khi đăng nhập thành công
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsLoginValid(string username, string password)
        {
            // Giả sử bạn đã cấu hình DbContext và kết nối tới cơ sở dữ liệu

            using (var context = new QlShopBanGiayContext()) 
            {
                var user = context.Users
                    .FirstOrDefault(u => u.Username == username && u.Password == password);

                return user != null;
            }
        }

    }
}
