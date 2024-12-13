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
    public partial class frm_Login : Form
    {
        public static string LoggedInUsername { get; set; }
        private readonly UserService _userService;
        private readonly IUserRepository _userRepository;
        public frm_Login(UserService userService)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            btn_DangNhap.Click += Btn_DangNhap_Click;
            btn_Thoat.Click += Btn_Thoat_Click;
            _userService = userService;
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
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
                    var user = _userService.GetAllUsers().FirstOrDefault(u => u.Username == username);

                    if (user != null)
                    {
                        // Lưu tên người dùng vào LoggedInUsername
                        LoggedInUsername = username;
                        if (user.Roleid == 1) 
                        {
                            MessageBox.Show("Chào mừng Quản trị viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (user.Roleid == 2)
                        {
                            MessageBox.Show("Chào mừng Nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        this.Hide();
                        frm_Main mainForm = new frm_Main(_userService);
                        mainForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin người dùng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng! Hoặc bạn không có quyền!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
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

        private void frm_Login_Load(object sender, EventArgs e)
        {

        }
    }
}
