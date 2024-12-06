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
using QLBanGiay.Models.Models;
using QLBanGiay_Application.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace QLBanGiay_Application.View
{
    public partial class frm_Users : Form
    {
        private readonly UserService _userService;
        private readonly QlShopBanGiayContext _context;
        private readonly RoleService _roleService;
        public frm_Users()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Load += Frm_Users_Load;
            this.btn_Xoa.Click += Btn_Xoa_Click;
            this.btn_Timkiem.Click += Btn_Timkiem_Click;
            this.dgv_danhsachnd.CellClick += Dgv_danhsachnd_CellClick;
            this.cbo_Mavaitro.SelectedIndexChanged += Cbo_Mavaitro_SelectedIndexChanged;
            this.btn_Thoat.Click += Btn_Thoat_Click;
            this.btn_Capnhat.Click += Btn_Capnhat_Click;
            this.btn_Reset.Click += Btn_Reset_Click;
            this.ck_Hoatdong.CheckedChanged += Ck_Hoatdong_CheckedChanged;
            this.ck_Bikhoa.CheckedChanged += Ck_Bikhoa_CheckedChanged;
            _context = new QlShopBanGiayContext();
            _userService = new UserService(new UserRepository(_context));
            _roleService = new RoleService(new RoleRepository(_context));

        }

        private void Ck_Bikhoa_CheckedChanged(object? sender, EventArgs e)
        {
            if (ck_Bikhoa.Checked)
            {
                ck_Hoatdong.Checked = false; 
            }
        }

        private void Ck_Hoatdong_CheckedChanged(object? sender, EventArgs e)
        {
            if (ck_Hoatdong.Checked)
            {
                ck_Bikhoa.Checked = false; 
            }
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            var hashedPassword = HashPassword(password); // Gọi phương thức mã hóa mật khẩu
            return _context.Users.FirstOrDefault(u => u.Username == username && u.Password == hashedPassword);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void Btn_Reset_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để reset mật khẩu!");
                return;
            }

            long userId = Convert.ToInt64(txt_Manguoidung.Text);

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn đặt lại mật khẩu cho người dùng này?",
                                          "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    string defaultPassword = "123";
                    user.Password = HashPassword(defaultPassword); 

                    _userService.UpdateUser(user);
                    MessageBox.Show("Reset mật khẩu thành công!");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng!");
                }
            }
        }
        //private void Btn_Reset_Click(object? sender, EventArgs e)
        //{
        //    if (string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
        //    {
        //        MessageBox.Show("Vui lòng chọn người dùng để reset mật khẩu!");
        //        return;
        //    }

        //    long userId = Convert.ToInt64(txt_Manguoidung.Text);

        //    var confirm = MessageBox.Show("Bạn có chắc chắn muốn đặt lại mật khẩu cho người dùng này?",
        //                                  "Xác nhận", MessageBoxButtons.YesNo);
        //    if (confirm == DialogResult.Yes)
        //    {
        //        var user = _userService.GetUserById(userId);
        //        if (user != null)
        //        {
        //            string defaultPassword = "123";  
        //            user.Password = defaultPassword;  

        //            _userService.UpdateUser(user);  
        //            MessageBox.Show("Reset mật khẩu thành công!");
        //            LoadUsers();  
        //        }
        //        else
        //        {
        //            MessageBox.Show("Không tìm thấy người dùng!");
        //        }
        //    }
        //}

        private void Btn_Capnhat_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để cập nhật!");
                return;
            }

            long userId = Convert.ToInt64(txt_Manguoidung.Text);

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn cập nhật thông tin người dùng này?",
                                          "Xác nhận", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var user = _userService.GetUserById(userId);
                if (user != null)
                {
                    
                    if (user.Roleid == 1)  
                    {         
                        ck_Bikhoa.Checked = false;  
                        ck_Bikhoa.Enabled = false;  
                    }
                    else
                    {
                        ck_Bikhoa.Enabled = true;  // Nếu không phải admin, cho phép thay đổi
                    }

                    user.Isactive = ck_Hoatdong.Checked;
                    user.Isbanned = ck_Bikhoa.Checked;

                    _userService.UpdateUser(user);
                    MessageBox.Show("Cập nhật người dùng thành công!");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy người dùng!");
                }
            }
        }

        private void Btn_Thoat_Click(object? sender, EventArgs e)
        {
            this.Close();
            frm_Main mainForm = new frm_Main();
            mainForm.Show();
        }
        private void Dgv_danhsachnd_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgv_danhsachnd.Rows[e.RowIndex];

                long userId = Convert.ToInt64(row.Cells["MãNgườiDùng"].Value);
                txt_Manguoidung.Text = row.Cells["MãNgườiDùng"].Value.ToString();
                txt_Tennguoidung.Text = row.Cells["TênNgườiDùng"].Value.ToString();
                cbo_Mavaitro.SelectedValue = row.Cells["VaiTrò"].Value;
                ck_Hoatdong.Checked = Convert.ToBoolean(row.Cells["HoạtĐộng"].Value);
                ck_Bikhoa.Checked = Convert.ToBoolean(row.Cells["BịKhóa"].Value);

                var user = _userService.GetUserById(userId);
                if (user != null && user.Roleid == 1)  
                {
                    ck_Bikhoa.Enabled = false;  
                    ck_Bikhoa.Checked = false;  
                }
                else
                {
                    ck_Bikhoa.Enabled = true; 
                }
            }
        }

        private void Cbo_Mavaitro_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cbo_Mavaitro.SelectedValue != null && long.TryParse(cbo_Mavaitro.SelectedValue.ToString(), out var roleId))
            {
                MessageBox.Show($"Bạn đã chọn vai trò có ID: {roleId}");
            }
        }

        private void Btn_Timkiem_Click(object? sender, EventArgs e)
        {
            var keyword = txt_Timkiem.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                MessageBox.Show("Vui lòng nhập từ khóa tìm kiếm!");
                return;
            }

            var users = _userService.GetAllUsers()
                                    .Where(u => u.Username.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                                    .ToList();

            if (!users.Any())
            {
                MessageBox.Show("Không tìm thấy người dùng nào!");
                return;
            }

            dgv_danhsachnd.DataSource = users.Select(u => new
            {
                MãNgườiDùng = u.Userid,
                TênNgườiDùng = u.Username,
                VaiTrò = u.Roleid,
                HoạtĐộng = u.Isactive,
                BịKhóa = u.Isbanned
            }).ToList();
        }

        private void Btn_Xoa_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa!");
                return;
            }

            long userId = Convert.ToInt64(txt_Manguoidung.Text);
            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng này?", "Xác nhận", MessageBoxButtons.YesNo);

            if (confirm == DialogResult.Yes)
            {
                _userService.DeleteUser(userId);
                MessageBox.Show("Xóa người dùng thành công!");
                LoadUsers();
            }
        }
        private bool ValidateInput(bool isUpdateOrDelete = false)
        {
            if (isUpdateOrDelete && string.IsNullOrWhiteSpace(txt_Manguoidung.Text))
            {
                MessageBox.Show("Vui lòng chọn người dùng để thực hiện thao tác!");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txt_Tennguoidung.Text))
            {
                MessageBox.Show("Vui lòng nhập tên người dùng!");
                return false;
            }

            return true;
        }
     
        private void Frm_Users_Load(object? sender, EventArgs e)
        {
            txt_Tennguoidung.Enabled = false;
            LoadUsers();
            LoadRoles();
        }
        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();

            if (users == null || !users.Any())
            {
                dgv_danhsachnd.DataSource = null;
                MessageBox.Show("Không có người dùng nào trong hệ thống!");
                return;
            }

            dgv_danhsachnd.DataSource = users.Select(u => new
            {
                MãNgườiDùng = u.Userid,
                TênNgườiDùng = u.Username,
                VaiTrò = u.Roleid,
                HoạtĐộng = u.Isactive,
                BịKhóa = u.Isbanned
            }).ToList();
        }
        private void LoadRoles()
        {
            var roles = _roleService.GetAllRoles(); 

            if (roles == null || !roles.Any())
            {
                MessageBox.Show("Không có vai trò nào trong hệ thống!");
                return;
            }

            cbo_Mavaitro.DataSource = roles;
            cbo_Mavaitro.DisplayMember = "RoleName";  
            cbo_Mavaitro.ValueMember = "Roleid";    

            cbo_Mavaitro.SelectedIndex = 0;
        }
    }
}
